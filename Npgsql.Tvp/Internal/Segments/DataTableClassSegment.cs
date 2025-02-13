using Npgsql.Internal;

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
        /// Sum 
        /// of the sizes of 
        /// the headers for 
        /// a single column.
        /// </summary>
        /// 
        /// 
        /// <remarks>
        /// See also: <seealso cref="SizeHeadersOveral"/>.
        /// </remarks>
        public static int SizeHeaders
        {
            get => sizeof(uint) + sizeof(int);
        }

        /// <summary>
        /// Sum of the sizes for all column headers.
        /// </summary>
        /// 
        /// <remarks>
        /// See also: <seealso cref="SizeOverall"/>.
        /// </remarks>
        public int SizeHeadersOveral
        {
            get => sizeof(int) + Length * SizeHeaders;
        }

        /// <summary>
        /// Sum of the headers and payload sizes.
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

        public DataTableClassSegment(ArraySegment<DataTableFieldSegment> fieldBuffer, PgSerializerOptions options, DataRow row)
        {
            _fieldBuffer = fieldBuffer;

            for (int i = 0; i < Length; i++)
            {
                _fieldBuffer[i] = new DataTableFieldSegment(row[row.Table.Columns[i]], options.GetDefaultTypeInfo(row.Table.Columns[i].DataType));

                if (!_fieldBuffer[i].IsNull)
                {
                    SizeOverall += _fieldBuffer[i].SizePayload;
                }
            }

            SizeOverall += SizeHeadersOveral;
        }
    }
}