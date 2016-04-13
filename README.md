
#### Connect MySql using ADO.NET

```fsharp
#r "packages/MySql.Data/lib/net45/MySql.Data.dll"
#r "System.Data.dll"
#r "System.Configuration.dll";

open MySql.Data.MySqlClient
open System.Data
open System

module MySqlExample =
    let connect() =
        let connectionString = "Server=localhost;UserId=root;Password=1234;Database=MySqlExample"
        use conn = new MySqlConnection(connectionString)
        conn.Open()

        let cmd = new MySqlCommand( "SELECT * FROM Customers", conn )
        let reader = cmd.ExecuteReader()

        while reader.Read()
          do System.Console.Write (reader.GetString "firstName" )
             System.Console.Write ("\t")
             System.Console.Write (reader.GetString "lastName" )
             System.Console.Write ("\t")
             System.Console.Write (reader.GetString "customerId")
MySqlExample.connect()
```