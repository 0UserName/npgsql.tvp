using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;

using System;

namespace Npgsql.Tvp.Internal.Segments
{
    internal sealed class DataTableFieldSegment(object payload, PgTypeInfo pgTypeInfo)
    {
        private object _writeState;

        /// <summary>
        /// Sum of the column header sizes.
        /// </summary>
        public static int SizeHeaders
        {
            get => sizeof(int) + sizeof(int);
        }

        /// <summary>
        /// Size of the column value.
        /// </summary>
        public int SizePayload
        {
            get => CalculateSizePayload();
        }

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
        public object Payload
        {
            get => payload;
        }

        /// <summary>
        /// True when the column value is <see cref="DBNull"/>.
        /// </summary>
        public bool IsNull
        {
            get => Payload == DBNull.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public PgConverter Converter
        {
            get => pgTypeInfo.GetObjectResolution(Payload).Converter;
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferRequirements BufferRequirements
        {
            get => pgTypeInfo.GetBufferRequirements(Converter, DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: { pgTypeInfo.PgTypeId }");
        }

        /// <summary>
        /// 
        /// </summary>
        public object WriteState
        {
            get => _writeState;
        }

        /// <summary>
        /// Calculates the size 
        /// of the column value 
        /// in bytes. 
        /// </summary>
        private int CalculateSizePayload()
        {
            if (IsNull)
            {
                return -1;
            }

            return BufferRequirements.Write.Kind == SizeKind.Exact ? BufferRequirements.Write.Value : _PgConverter.GetSize(Converter, default, Payload, ref _writeState).Value;
        }
    }
}