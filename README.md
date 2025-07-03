![Release workflow](https://github.com/0UserName/npgsql.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

The motivation for developing this plugin is to emulate table-valued parameters, which are [supported](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/table-valued-parameters) by SQL Server, but [not](https://github.com/npgsql/npgsql/issues/5415) by PostgreSQL, thereby enhancing code portability.



# Usage

Add a dependency on this package and create an `NpgsqlDataSource`. Once this is done, you can use `DataTable` and `DbDataReader` types when interacting with PostgreSQL:



```csharp
NpgsqlDataSourceBuilder builder = new NpgsqlDataSourceBuilder("Host=localhost;Username=postgres;Password=root;Database=postgres");

builder.UseTvp();

NpgsqlDataSource dataSource = builder.Build();

NpgsqlConnection connection = dataSource.CreateConnection();

await connection.OpenAsync();

DataTable dt = new DataTable("schema.compositeType"); // TableName is required!

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");

using (NpgsqlCommand cmd = new NpgsqlCommand($"CALL schema.storedProcedure(@{ nameof(dt) })", connection))
{
    cmd.Parameters.Add(new NpgsqlParameter(nameof(dt), dt));

    await cmd.ExecuteNonQueryAsync();
}
```



The parameter is processed by the driver as an array of a composite type that was previously created on the server:



```sql
CREATE PROCEDURE schema.storedProcedure(IN params schema.compositeType[])
```



# Related Projects

- [Simple.DbExtensions.Tvp](https://github.com/0UserName/Simple.DbExtensions.Tvp)