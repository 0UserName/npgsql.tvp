using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object item, PgConverterResolution resolution)
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
            get => resolution.PgTypeId.Oid.Value;
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
            get => resolution.Converter;
        }

        public BufferRequirements BufferRequirements
        {
            get => Converter.CanConvert(DataFormat.Binary, out BufferRequirements bufferRequirements) ? bufferRequirements : throw new NotSupportedException($"Binary format is not supported: { resolution.PgTypeId }");
        }
    }
}