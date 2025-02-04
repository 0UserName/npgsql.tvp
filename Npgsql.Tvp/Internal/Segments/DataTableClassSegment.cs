using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.PostgresTypes;

using System;
using System.Collections;
using System.Collections.Generic;

using System.Data;

namespace Npgsql.Tvp.Internal.Segments
{
    internal sealed class DataTableClassSegment : IEnumerable<DataTableFieldSegment>
    {
        private readonly ArraySegment<DataTableFieldSegment> _fieldBuffer;

        /// <summary>
        /// Sum of 
        /// the headers 
        /// sizes.
        /// </summary>
        /// 
        /// <remarks>
        /// See also: <seealso cref="SizeOverall"/>.
        /// </remarks>
        public int SizeHeaders
        {
            get => sizeof(int) + Length * (sizeof(uint) + sizeof(int));
        }

        /// <summary>
        /// Sum of 
        /// the headers 
        /// and payload 
        /// sizes. 
        /// </summary>
        /// 
        /// <remarks>
        /// See also: <seealso cref="DataTableFieldSegment.SizePayload"/>.
        /// </remarks>
        public int SizeOverall
        {
            get;
            private set;
        }

        /// <summary>
        /// Total number of columns.
        /// </summary>
        public int Length
        {
            get => _fieldBuffer.Count;
        }

        /// <inheritdoc/>
        public IEnumerator<DataTableFieldSegment> GetEnumerator()
        {
            return _fieldBuffer.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DataTableClassSegment(ArraySegment<DataTableFieldSegment> fieldBuffer, PostgresCompositeType pgType, PgSerializerOptions options, DataColumnCollection columns, DataRow row)
        {
            _fieldBuffer = fieldBuffer;

            for (int i = 0; i < Length; i++)
            {
                _fieldBuffer[i] = new DataTableFieldSegment(row[columns[pgType.Fields[i].Name]], options.GetDefaultTypeInfo((Oid)pgType.Fields[i].Type.OID));

                if (!_fieldBuffer[i].IsNull)
                {
                    SizeOverall += _fieldBuffer[i].SizePayload;
                }
            }

            SizeOverall += SizeHeaders;
        }
    }
}