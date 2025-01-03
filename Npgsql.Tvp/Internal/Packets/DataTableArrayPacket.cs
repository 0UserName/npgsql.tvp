using Npgsql.Internal;
using Npgsql.PostgresTypes;

using System;
using System.Buffers;

using System.Collections;
using System.Collections.Generic;

using System.Data;

namespace Npgsql.Tvp.Internal.Packets
{
    internal sealed class DataTableArrayPacket : IEnumerable<DataTablePacket>, IDisposable
    {
        public const int HEADER_SIZE_ARRAY_DIMENSIONS     = sizeof(int);
        public const int HEADER_SIZE_ARRAY_FLAGS          = sizeof(int);
        public const int HEADER_SIZE_ARRAY_ELEMENT_OID    = sizeof(uint);
        public const int HEADER_SIZE_ARRAY_LENGTH         = sizeof(int);
        public const int HEADER_SIZE_ARRAY_LOWER_BOUND    = sizeof(int);
        public const int HEADER_SIZE_ARRAY_ELEMENT_LENGTH = sizeof(int);

        /// <summary>
        /// 
        /// </summary>
        private readonly ArraySegment<DataTablePacket> _segments;

        /// <summary>
        /// 
        /// </summary>
        public int SizeHeaders
        {
            get => HEADER_SIZE_ARRAY_DIMENSIONS + HEADER_SIZE_ARRAY_FLAGS + HEADER_SIZE_ARRAY_ELEMENT_OID + HeaderArrayDimensions * (HEADER_SIZE_ARRAY_LENGTH + HEADER_SIZE_ARRAY_LOWER_BOUND) + HEADER_SIZE_ARRAY_ELEMENT_LENGTH * _segments.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        ///
        /// <remarks>
        /// Only flat arrays are supported.
        /// </remarks>
        public int HeaderArrayDimensions
        {
            get => 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Not really used.
        /// </remarks>
        public int HeaderArrayFlags
        {
            get => 0;
        }

        /// <summary>
        /// Element OID.
        /// </summary>
        public uint HeaderArrayElementOid
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ArraySomething
        {
            get => (HEADER_SIZE_ARRAY_LENGTH + HEADER_SIZE_ARRAY_LOWER_BOUND) * HeaderArrayDimensions;
        }

        /// <summary>
        /// 
        /// </summary>
        public int HeaderArrayLenght
        {
            get => _segments.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public int HeaderArrayLowerbound
        {
            get => 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public Size SizeCompositeHeaders
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Size SizeTotal
        {
            get;
            private set;
        }

        /// <summary>
        /// Throws the exception if the fields 
        /// of the composite type do not match
        /// the columns of the table.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException"/>
        /// 
        /// <remarks>
        /// If there are columns in the 
        /// table for all fields of the composite type, 
        /// then an implicit trim is performed for the 
        /// columns outside the type.
        /// </remarks>
        private void ThrowIfFieldMismatch(DataTable table, PostgresCompositeType type)
        {
            for (int i = 0; i < type.Fields.Count; i++)
            {
                if (!type.Fields[i].Name.Equals(table.Columns[i].ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new InvalidOperationException($"The number of columns in the table [{table.TableName}] does not match the number of fields in the composite type [{type.DisplayName}]: {table.Columns.Count} vs. {type.Fields.Count}");
                }
            }
        }

        /// <summary>
        /// Calculates the size of <see cref="DataColumn"/>
        /// headers as if each were a composite type field.
        /// </summary>
        ///
        /// <remarks>
        /// Calculation is performed for each row.
        /// </remarks>
        private Size CalculateCompositeHeadersSize()
        {
            Size total = 0;

            for (int i = 0; i < _segments.Count; i++)
            {
                total = total.Combine(_segments[i].SizeHeaders);
            }

            return total;
        }

        /// <inheritdoc/>
        public IEnumerator<DataTablePacket> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ArrayPool<DataTablePacket>.Shared.Return(_segments.Array, true);
        }

        public DataTableArrayPacket(PostgresCompositeType type, PgSerializerOptions options, DataTable table)
        {
            ThrowIfFieldMismatch(table, type);

            _segments = new ArraySegment<DataTablePacket>(ArrayPool<DataTablePacket>.Shared.Rent(table.Rows.Count), 0, table.Rows.Count);

            HeaderArrayElementOid = type.OID;

            int total = 0;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                _segments[i] = new DataTablePacket(type, options, table.Columns, table.Rows[i]);

                total += _segments[i].SizeOverall;
            }

            SizeCompositeHeaders = CalculateCompositeHeadersSize();

            SizeTotal = Size.Create(total).Combine(SizeHeaders);
        }
    }
}