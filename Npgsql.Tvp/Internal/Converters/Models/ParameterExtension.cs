﻿using System.Data;
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
            return table.TableName != "SchemaTable" ? table.TableName : table.Columns[SchemaTableColumn.BaseTableName].DefaultValue as string;
        }

        /// <summary>
        /// Gets the name of the schema table.
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