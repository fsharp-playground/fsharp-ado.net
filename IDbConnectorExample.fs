open System
open MySql.Data.MySqlClient
open System.Data.SqlClient
open Npgsql
open System.Data
open System.Linq

type ConnectionString =
    { Value : string }

type Field =
    { Type : string
      Value : string
      Name : string }

type QueryResult =
    { Success : bool
      Message : string
      Fields : Field list }

type Db =
    | MYSQL of ConnectionString
    | POSTGRESQL of ConnectionString
    | MSSQL of ConnectionString

    static member CreateConnector(db : Db) =
        match db with
        | MYSQL p -> new MySqlConnection(p.Value) :> IDbConnection
        | MSSQL p -> new SqlConnection(p.Value) :> IDbConnection
        | POSTGRESQL p -> new NpgsqlConnection(p.Value) :> IDbConnection

    static member Query sql (db : Db) =
        use conn = Db.CreateConnector(db)
        use command = conn.CreateCommand()
        do command.CommandText <- sql
           conn.Open()
        use reader = command.ExecuteReader()

        let createResult (reader : IDataReader) index =
            let value = reader.GetValue index
            let name = reader.GetName index
            let fieldType = reader.GetFieldType index
            { Field.Name = name
              Value = value.ToString()
              Type = fieldType.ToString() }

        let map (reader : IDataReader) =
            let count = reader.FieldCount
            let range = Enumerable.Range(0, count).ToList()
            range.Select(fun x -> createResult reader x) |> Seq.toList

        let result =
            if reader.Read() then
                { QueryResult.Success = true
                  Message = ""
                  Fields = reader |> map }
            else
                { QueryResult.Success = false
                  Message = ""
                  Fields = [] }

        (result)