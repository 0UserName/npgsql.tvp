![Release workflow](https://github.com/0UserName/npgsql.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

The motivation for developing this library is to enable the use of `DataTable` and `DbDataReader` when interacting with PostgreSQL, which currently lacks such support [[#5415](https://github.com/npgsql/npgsql/issues/5415)].



# Usage

Add a dependency on this package and create an `NpgsqlDataSource`. Once this is done, you can use `DataTable` and `DbDataReader` types when interacting with PostgreSQL:



```csharp
NpgsqlDataSourceBuilder builder = new
NpgsqlDataSourceBuilder
("Host=localhost;Username=postgres;Password=root;Database=postgres");

builder.UseTvp();

NpgsqlDataSource dataSource = builder.Build();

NpgsqlConnection connection = dataSource.CreateConnection();

await connection.OpenAsync();

DataTable dt = new
DataTable
("schema.compositeType"); // TableName is required!

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