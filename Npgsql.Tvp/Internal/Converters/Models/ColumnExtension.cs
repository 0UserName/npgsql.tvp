using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class ColumnExtension
    {
        /// <summary>
        /// Checks if the column value 
        /// is <see cref="DBNull.Value"/>.
        /// </summary>
        public static bool IsDbNull(this in Column column)
        {
            return column.Value == DBNull.Value;
        }

        /// <summary>
        /// Gets the size of 
        /// the column value 
        /// in bytes. 
        /// </summary>
        public static int GetSizeOrDbNullAsObject(this in Column column, object writeState)
        {
            if (column.IsDbNull())
            {
                return -1;
            }

            return column.GetBufferRequirements().Write.Kind == SizeKind.Exact ? column.GetBufferRequirements().Write.Value : _PgConverter.GetSizeAsObject(column.GetConverter(), default, column.Value, ref writeState).Value;
        }
    }
}