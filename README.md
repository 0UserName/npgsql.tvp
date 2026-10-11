![Release workflow](https://github.com/0UserName/npgsql.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

<div align="justify">

The plugin enables the use of table-valued parameters in `PostgreSQL` by emulating `SQL Server`'s table-valued parameters, which are [not](https://github.com/npgsql/npgsql/issues/5415) natively supported by `PostgreSQL`.

</div>



# Usage

<div align="justify">

Add a dependency on this package and configure an `NpgsqlDataSource` using `NpgsqlDataSourceBuilder`. Once configured, `IDataReader`-based and `DataTable`-based parameters can be used when interacting with `PostgreSQL`:

</div>



```csharp
NpgsqlDataSourceBuilder builder = new
NpgsqlDataSourceBuilder
("Host=localhost;Username=postgres;Password=root;Database=postgres");

builder.UseTvp();

// Connect..

DataTable dt = new
DataTable
("schema.compositeType"); // TableName is required!

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");

NpgsqlCommand cmd = new
NpgsqlCommand
("schema.storedProcedure", connection);

cmd.Parameters.Add(new NpgsqlParameter(nameof(dt), dt));

// Set command type and execute..
```



<div align="justify">

The plugin processes the parameter as an array of a composite type previously created on the server:

</div>



```sql
CREATE PROCEDURE schema.storedProcedure(IN dt schema.compositeType[])
```



# Metadata

<div align="justify">

Internally, the plugin relies on parameter metadata, including the type name ([BaseTableName](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.basetablename?view=net-10.0)) and column names ([ColumnName](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.columnname?view=net-10.0)), data types ([DataType](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.datatype?view=net-10.0)), and nullability ([AllowDBNull](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.allowdbnull?view=net-10.0)). The parameter type name is used to look up the corresponding `OID` in the driver's type metadata cache, while column data types and nullability are used to determine whether the size of each row is variable or constant.

</div>



> [!WARNING]
> Specifying the parameter type using [NpgsqlParameter.DataTypeName](https://www.npgsql.org/doc/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter_DataTypeName) is not supported.



## IDataReader

<div align="justify">

For `IDataReader`-based parameters, metadata must be provided through the [GetSchemaTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.idatareader.getschematable?view=net-10.0).

</div>



## DataTable

<div align="justify">

For `DataTable`-based parameters, the plugin retrieves metadata directly from the table, since it is structure is self-describing.

</div>



# Performance considerations


## Expression trees

<div align="justify">

Row values are accessed through compiled lambda expressions that are created for each type and cached for reuse. These lambdas encapsulate property getters and null-checks, eliminating reflection overhead.

</div>



## Row sizes

<div align="justify">

The plugin optimizes buffer usage during remote calls by classifying parameters as variable-size or constant-size based on column metadata ([DataType](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.datatype?view=net-10.0) and [AllowDBNull](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.allowdbnull?view=net-10.0)) and the number of rows in the parameter. Variable-size data is first materialized in an intermediate buffer to determine the size of each row before being written to the driver's internal buffer. Constant-size data is streamed directly into that buffer because the size of each row is known in advance.

</div>



> [!WARNING]
> For `IDataReader`-based parameters, the `RecordsAffected` property should returns the total number of rows rather than the number of rows affected by DML operations.



# References

- [Table-Valued Parameters - ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/table-valued-parameters)