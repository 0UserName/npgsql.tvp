using System.Data;
using System.Data.Common;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class ParameterExtension
    {
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public static string GetDataTypeName(this DataTable table)
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
        public static string GetDataTypeName(this DbDataReader reader)
        {
            return reader.GetSchemaTable().GetDataTypeName();
        }
    }
}