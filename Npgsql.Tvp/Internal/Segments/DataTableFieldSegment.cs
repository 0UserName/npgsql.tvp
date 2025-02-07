using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;
using Npgsql.Tvp.Internal.Segments.Abstract;

using System;

namespace Npgsql.Tvp.Internal.Segments
{
    internal sealed class DataTableFieldSegment(object payload, PgTypeInfo pgTypeInfo) : AbstractTyped(pgTypeInfo.PgTypeId.Value.Oid.Value)
    {
        private object _writeState;

        /// <summary>
        /// Size of the column value.
        /// </summary>
        public int SizePayload
        {
            get => CalculateSizePayload();
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