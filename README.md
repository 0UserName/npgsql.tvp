![Release workflow](https://github.com/0UserName/npgsql.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

The motivation for developing the library is to allow the use of the `DataTable` when interacting with PostgreSQL, which lacks such support [[#5415](https://github.com/npgsql/npgsql/issues/5415)].



# Usage

Add a dependency on this package and create an `NpgsqlDataSource`. Once this is done, you can use `DataTable` types when interacting with PostgreSQL:



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
("schema_compositeType"); // TableName is required!

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");

using (NpgsqlCommand cmd = new
       NpgsqlCommand($"CALL schema.storedProcedure(@{ nameof(dt) })", connection))
{
    cmd.Parameters.Add(new NpgsqlParameter(nameof(dt), dt));

    cmd.ExecuteNonQuery();
}
```



# Related projects

- [Simple.DbExtensions.Tvp](https://github.com/0UserName/Simple.DbExtensions.Tvp)