using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;
using System.Data;
using System.Data.Common;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class ParameterExtension
    {
        /// <summary>
        /// See: <see href="https://www.postgresql.org/docs/current/sql-createtype.html#SQL-CREATETYPE-ARRAY">Array Types</see>.
        /// </summary>
        private static PgTypeId GetArrayDataType(string dataTypeName, PgSerializerOptions options)
        {
            return options.GetArrayTypeId(new DataTypeName(dataTypeName ?? throw new InvalidOperationException("The parameter must provide the data type name")));
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        private static string GetDataTypeName(this DataTable table)
        {
            return table.TableName != "SchemaTable" ? table.TableName : (string)table.Columns[SchemaTableColumn.BaseTableName].DefaultValue;
        }

        /// <summary>
        /// Gets the name of the base table.
        /// </summary>
        /// 
        /// <remarks>
        /// Relies on <see cref="IDataReader.GetSchemaTable()"/>.
        /// </remarks>
        private static string GetDataTypeName(this DbDataReader reader)
        {
            return reader.GetSchemaTable().GetDataTypeName();
        }

        /// <inheritdoc cref="GetArrayDataType(string, PgSerializerOptions)"/>
        public static PgTypeId GetArrayType(this DataTable table, PgSerializerOptions options)
        {
            return GetArrayDataType(table.GetDataTypeName(), options);
        }

        /// <inheritdoc cref="GetArrayDataType(string, PgSerializerOptions)"/>
        public static PgTypeId GetArrayType(this DbDataReader reader, PgSerializerOptions options)
        {
            return GetArrayDataType(reader.GetDataTypeName(), options);
        }
    }
}