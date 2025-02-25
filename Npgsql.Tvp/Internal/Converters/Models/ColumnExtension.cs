using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class ColumnExtension
    {
        /// <summary>
        /// Gets the size of 
        /// the column value 
        /// in bytes. 
        /// </summary>
        [Obsolete("Used until the same method from Npgsql become public")]
        public static int GetSizeOrDbNullAsObject(this in Column column, object writeState)
        {
            if (column.IsDbNull)
            {
                return -1;
            }

            return column.BufferRequirements.Write.Kind == SizeKind.Exact ? column.BufferRequirements.Write.Value : _PgConverter.GetSizeAsObject(column.Converter, default, column.Value, ref writeState).Value;
        }
    }
}