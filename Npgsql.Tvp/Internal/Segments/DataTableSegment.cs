using Npgsql.Internal;

using Npgsql.Tvp.Internal.Segments.Abstract;

using System;
using System.Buffers;

using System.Collections;
using System.Collections.Generic;

using System.Data;

namespace Npgsql.Tvp.Internal.Segments
{
    internal sealed class DataTableSegment : AbstractTyped, IEnumerable<DataTableClassSegment>, IDisposable
    {
        private readonly DataTableClassSegment[] _classBuffer;
        private readonly DataTableFieldSegment[] _fieldBuffer;

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
            get => sizeof(int) + sizeof(int) + sizeof(uint) + Dimensions * (sizeof(int) + sizeof(int)) + sizeof(int) * Length;
        }

        /// <summary>
        /// Sum of 
        /// the headers 
        /// and payload 
        /// sizes. 
        /// </summary>
        public int SizeOverall
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of dimensions.
        /// </summary>
        ///
        /// <remarks>
        /// Only one dimension is supported.
        /// </remarks>
        public int Dimensions
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
        public int Flags
        {
            get => 0;
        }

        /// <summary>
        /// Total number of rows.
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// Index of the first row.
        /// </summary>
        public int LowerBound
        {
            get => 1;
        }

        /// <inheritdoc/>
        public IEnumerator<DataTableClassSegment> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return _classBuffer[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ArrayPool<DataTableClassSegment>.Shared.Return(_classBuffer, true);
            ArrayPool<DataTableFieldSegment>.Shared.Return(_fieldBuffer, true);
        }

        public DataTableSegment(PgSerializerOptions options, DataTable dt) : base(default)
        {
            Length = dt.Rows.Count;

            _classBuffer = ArrayPool<DataTableClassSegment>.Shared.Rent(Length);
            _fieldBuffer = ArrayPool<DataTableFieldSegment>.Shared.Rent(Length * dt.Columns.Count);

            for (int i = 0; i < Length; i++)
            {
                _classBuffer[i] = new DataTableClassSegment(new ArraySegment<DataTableFieldSegment>(_fieldBuffer, i * dt.Columns.Count, dt.Columns.Count), options, dt.Columns, dt.Rows[i]);

                SizeOverall += _classBuffer[i].SizeOverall;
            }

            SizeOverall += SizeHeaders;
        }
    }
}