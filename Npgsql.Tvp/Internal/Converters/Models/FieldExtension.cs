using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class FieldExtension
    {
        /// <summary>
        /// Checks if the column value 
        /// is <see cref="DBNull.Value"/>.
        /// </summary>
        public static bool IsDbNull(this in Field field)
        {
            return field.Value == DBNull.Value;
        }

        /// <summary>
        /// Gets the size of 
        /// the column value 
        /// in bytes. 
        /// </summary>
        public static int GetSizeOrDbNullAsObject(this in Field field, object writeState)
        {
            if (field.IsDbNull())
            {
                return -1;
            }

            return field.GetBufferRequirements().Write.Kind == SizeKind.Exact ? field.GetBufferRequirements().Write.Value : _PgConverter.GetSizeAsObject(field.GetConverter(), default, field.Value, ref writeState).Value;
        }
    }
}