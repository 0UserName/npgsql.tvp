using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Abstract
{
    internal abstract class AbstractParameter(string dataTypeName, int columnsCount, int rowsCount, PgSerializerOptions options) : IParameter
    {
        /// <inheritdoc/>
        public int ColumnsCount
        {
            get => columnsCount;
        }

        /// <inheritdoc/>
        public virtual int RowsCount
        {
            get => rowsCount;
        }

        /// <inheritdoc/>
        public int MetadataSize
        {
            get => sizeof(int) + sizeof(int) + sizeof(uint) + Constants.DIMENSIONS * (sizeof(int) + sizeof(int)) + sizeof(int) * RowsCount;
        }

        /// <inheritdoc/>
        public uint OID
        {
            get => options.GetArrayElementTypeId(new DataTypeName(dataTypeName).ToArrayName()).Oid.Value;
        }

        /// <inheritdoc/>
        public abstract Value this[int row, int column]
        {
            get;
        }

        /// <summary>
        /// Gets the offset 
        /// to the start of 
        /// the row.
        /// </summary>
        protected int GetY(int row)
        {
            return row * columnsCount;
        }

        /// <summary>
        /// Gets 
        /// the offset 
        /// within the 
        /// row.
        /// </summary>
        protected int GetYX(int row, int column)
        {
            return row * columnsCount + column;
        }

        /// <summary>
        /// Gets the offset to 
        /// the end of the row.
        /// </summary>
        protected int GetYX(int row)
        {
            return GetYX(row, columnsCount);
        }

        private static int GetItemSize(object item, PgConverter converter, Size writeRequirement)
        {
            object? writeState = default;

            return item is DBNull or null ? -1 : Accessors.GetSizeOrDbNullAsObject(default, converter, DataFormat.Binary, writeRequirement, item, ref writeState).Value.Value;
        }

        protected Value CreateValue(object item, Type type)
        {
            PgTypeInfo pgTypeInfo = options.GetDefaultTypeInfo(type) ?? throw new NotSupportedException($"Type is not supported: {type.FullName}");

            PgConverterResolution resolution = pgTypeInfo.GetObjectResolution(item); // {Npgsql.Internal.PgResolverTypeInfo} for DateTime

            BufferRequirements bufferRequirements = pgTypeInfo.GetBufferRequirements(resolution.Converter, DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: {pgTypeInfo.PgTypeId}");

            return new Value(item, resolution.PgTypeId.Oid.Value, resolution.Converter, bufferRequirements.Write, GetItemSize(item, resolution.Converter, bufferRequirements.Write));
        }

        /// <inheritdoc/>
        public int GetRowSize(int row)
        {
            int size = sizeof(int) + columnsCount * Value.METADATA_SIZE;

            for (int i = GetY(row); i < GetYX(row); i++)
            {
                size += Math.Max(0, this[row, i % columnsCount].Size);
            }

            return size;
        }

        /// <inheritdoc/>
        public int GetSize()
        {
            int size = MetadataSize;

            for (int i = 0; i < RowsCount; i++)
            {
                size += GetRowSize(i);
            }

            return size;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // NOP.
        }
    }
}