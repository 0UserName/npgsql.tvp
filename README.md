﻿This package is an Npgsql plugin which allows you to use the [DataTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-9.0) type when interacting with PostgreSQL.

# Motivation

The motivation for developing the plugin is a seamless transition from SQL Server, which natively supports `DataTable` as table-valued parameter, to PostgreSQL, which lacks this support [[#5415](https://github.com/npgsql/npgsql/issues/5415)].

# Usage

Add a dependency on this package and create a `NpgsqlDataSource`. Once this is done, you can use `DataTable` types when interacting with PostgreSQL:

```csharp
NpgsqlDataSourceBuilder builder = new
NpgsqlDataSourceBuilder
("Host=localhost;Username=postgres;Password=root;Database=postgres");

builder.UseTvp<DataTable>();

NpgsqlDataSource dataSource = builder.Build();

NpgsqlConnection connection = dataSource.CreateConnection();

await connection.OpenAsync();

DataTable dt = new
DataTable
("schema_compositeType");

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3");
dt.Rows.Add("Column1_value", "Column2_value", "Column3");
dt.Rows.Add("Column1_value", "Column2_value", "Column3");

using (NpgsqlCommand cmd = new NpgsqlCommand("CALL schema.storedProcedure(@arg)", connection))
{
    NpgsqlParameter arg = new
    NpgsqlParameter
    (nameof(arg), dt)
    {
        DataTypeName = "schema._compositeType"
    };

    cmd.Parameters.Add(arg);

    cmd.ExecuteNonQuery();
}
```

Initialization of `TableName` and `DataTypeName` is mandatory! The values must correspond to the name of the composite type in the database. Additionally, for `DataTypeName`, the array notation must be used: `schema._compositeType`, as the plugin essentially treats `DataTable` as an array of composite types.