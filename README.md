#### Connect MySql using ADO.NET

- ต้อง Add reference `System.Data.dll` และ `System.Configuration.dll`

```fsharp
#r "packages/MySql.Data/lib/net45/MySql.Data.dll"
#r "System.Data.dll"
#r "System.Configuration.dll";

open MySql.Data.MySqlClient

module MySqlExample =
    let connect() =
        let connectionString = "Server=localhost;UserId=root;Password=1234;Database=MySqlExample"
        use conn = new MySqlConnection(connectionString)
        conn.Open()

        let cmd = new MySqlCommand( "SELECT * FROM Customers", conn)
        let reader = cmd.ExecuteReader()

        while reader.Read()
          do printf "%s\t" <| reader.GetString "firstName"
             printf "%s\t" <| reader.GetString "lastName"
             printf "%s\n" <| reader.GetString "customerId"
MySqlExample.connect()
```
