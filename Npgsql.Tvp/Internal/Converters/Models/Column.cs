using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Column(object value, PgTypeInfo pgTypeInfo)
    {
        /// <summary>
        /// Size of the column headers.
        /// </summary>
        /// 
        /// <remarks>
        /// OID + Length.
        /// </remarks>
        public const int HEADERS_SIZE = sizeof(uint) + sizeof(int);

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
        /// 
        /// </summary>
        public bool IsDbNull
        {
            get => value == DBNull.Value;
        }

        /// <summary>
        /// Size of the column value.
        /// </summary>
        public int Size
        {
            get => this.GetSizeOrDbNullAsObject(default);
        }

        /// <summary>
        /// 
        /// </summary>
        public PgConverter Converter
        {
            get => pgTypeInfo.GetObjectResolution(value).Converter;
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferRequirements BufferRequirements
        {
            get => pgTypeInfo.GetBufferRequirements(Converter, DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: { pgTypeInfo.PgTypeId }");
        }
    }
}