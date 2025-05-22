using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Abstract
{
    internal abstract class AbstractParameter(string dataTypeName, PgSerializerOptions options) : IParameter
    {
        /// <summary>
        /// Gets type information for Npgsql.
        /// </summary>
        protected PgSerializerOptions Options
        {
            get => options;
        }

        /// <inheritdoc/>
        public abstract int ColumnsCount
        {
            get;
        }

        /// <inheritdoc/>
        public abstract int RowsCount
        {
            get;
        }

        /// <inheritdoc/>
        /// 
        /// <remarks>
        /// Dimensions + Flags + Parameter OID + $Dimensions * (array length and lower bound) + $Value length integers.
        /// </remarks>
        public int MetadataSize
        {
            get => sizeof(int) +
                   sizeof(int) + sizeof(uint) + Constants.DIMENSIONS * (sizeof(int) + sizeof(int)) +
                   sizeof(int) * RowsCount;
        }

        /// <inheritdoc/>
        public uint Oid
        {
            get => options.GetArrayElementTypeId(new DataTypeName(dataTypeName).ToArrayName()).Oid.Value;
        }

        /// <inheritdoc/>
        public abstract Value this[int row, int column]
        {
            get;
        }

        /// <inheritdoc/>
        public int GetRowSize(int row)
        {
            int size = sizeof(int) + ColumnsCount * Value.METADATA_SIZE;

            for (int i = row * ColumnsCount; i < row * ColumnsCount + ColumnsCount; i++)
            {
                size += Math.Max(0, this[row, i % ColumnsCount].Size);
            }

            return size;
        }

        /// <inheritdoc/>
        public int GetOverallSize()
        {
            int size = MetadataSize;

            for (int i = 0; i < RowsCount; i++)
            {
                size += GetRowSize(i);
            }

            return size;
        }
    }
}