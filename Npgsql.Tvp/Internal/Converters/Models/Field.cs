using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Field(object value, PgTypeInfo pgTypeInfo)
    {
        /// <summary>
        /// Size of the column headers.
        /// </summary>
        public const int HEADERS_SIZE = sizeof(uint) + sizeof(int); // OID + Length.

        /// <summary>
        /// A data type used for
        /// identifying internal 
        /// objects.
        /// </summary>
        public uint Oid
        {
            get => pgTypeInfo.PgTypeId.Value.Oid.Value;
        }

        /// <summary>
        /// Value stored in the column.
        /// </summary>
        public object Value
        {
            get => value;
        }

        /// <summary>
        /// Size of the column value.
        /// </summary>
        public int ValueSize
        {
            get => this.GetSizeOrDbNullAsObject(default);
        }

        /// <summary>
        /// 
        /// </summary>
        public PgConverter GetConverter()
        {
            return pgTypeInfo.GetObjectResolution(value).Converter;
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferRequirements GetBufferRequirements()
        {
            return pgTypeInfo.GetBufferRequirements(GetConverter(), DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: { pgTypeInfo.PgTypeId }");
        }
    }
}