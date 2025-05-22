using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object item, PgTypeInfo pgTypeInfo)
    {
        /// <summary>
        /// Size of the item headers.
        /// </summary>
        /// 
        /// <remarks>
        /// OID + Length.
        /// </remarks>
        public const int METADATA_SIZE = sizeof(uint) + sizeof(int);

        /// <summary>
        /// A data type used for
        /// identifying internal 
        /// objects.
        /// </summary>
        public uint Oid
        {
            get => pgTypeInfo.PgTypeId.Value.Oid.Value;
        }

        public object Item
        {
            get => item;
        }

        public bool IsDbNull
        {
            get => item == default || item == DBNull.Value;
        }

        /// <summary>
        /// Size of the item in bytes.
        /// </summary>
        public int Size
        {
            get => this.GetSizeOrDbNullAsObject(default);
        }

        public PgConverter Converter
        {
            get => pgTypeInfo.GetObjectResolution(item).Converter;
        }

        public BufferRequirements BufferRequirements
        {
            get => pgTypeInfo.GetBufferRequirements(Converter, DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: { pgTypeInfo.PgTypeId }");
        }
    }
}