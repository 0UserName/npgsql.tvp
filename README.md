![GitHub Build Status](https://github.com/0UserName/npgsql.tvp/actions/workflows/main.yml/badge.svg)

This package is an Npgsql plugin which allows you to use the [DataTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-9.0) type when interacting with PostgreSQL.

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
("schema_compositeType"); // TableName is required

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");

using (NpgsqlCommand cmd = new
       NpgsqlCommand("CALL schema.storedProcedure(@arg)", connection))
{
    cmd.Parameters.Add(new NpgsqlParameter(nameof(arg), dt));

    cmd.ExecuteNonQuery();
}
```