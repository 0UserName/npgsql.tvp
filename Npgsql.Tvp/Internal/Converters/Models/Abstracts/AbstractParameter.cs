using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Buffers;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Abstracts
{
    internal abstract class AbstractParameter(int columnsCount, int rowsCount, uint oid, PgSerializerOptions options) : IParameter
    {
        protected ValueBuffer Buffer
        {
            get => field ??= new ValueBuffer(columnsCount, rowsCount);
        }

        /// <inheritdoc/>
        public int ColumnsCount
        {
            get => columnsCount;
        }

        /// <inheritdoc/>
        public int RowsCount
        {
            get => Buffer.Written / columnsCount;
        }

        /// <inheritdoc/>
        public int MetadataSize
        {
            get => sizeof(int) + sizeof(int) + sizeof(uint) + (sizeof(int) + sizeof(int)) * Constants.DIMENSIONS + sizeof(int) * RowsCount;
        }

        /// <inheritdoc/>
        public uint OID
        {
            get => oid;
        }

        /// <inheritdoc/>
        public int this[int row]
        {
            get => Buffer[row];
        }

        /// <inheritdoc/>
        public Value this[int row, int column]
        {
            get => Buffer[row, column];
        }

        /// <summary>
        /// Copies values 
        /// from the input parameter 
        /// into the internal buffer.
        /// </summary>
        protected abstract void FillBuffer();

        /// <summary>
        /// Wraps 
        /// the value in a container with 
        /// metadata needed to send it to 
        /// the server.
        /// </summary>
        protected Value CreateValue(object value, Type type)
        {
            PgTypeInfo pgTypeInfo = options.GetDefaultTypeInfo(type);

            PgConverterResolution resolution = pgTypeInfo.GetObjectResolution(value);

            return new Value(value, resolution.PgTypeId.Oid.Value, resolution.Converter, pgTypeInfo.GetBufferRequirements(resolution.Converter, DataFormat.Binary).Value.Write);
        }

        /// <inheritdoc/>
        public int GetSize()
        {
            FillBuffer();

            return MetadataSize + Buffer.Size;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            Buffer.Dispose();
        }
    }
}