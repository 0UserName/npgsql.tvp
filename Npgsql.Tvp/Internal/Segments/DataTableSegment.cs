using Npgsql.Internal;

using System;
using System.Buffers;

using System.Collections;
using System.Collections.Generic;

using System.Data;

namespace Npgsql.Tvp.Internal.Segments
{
    internal sealed class DataTableSegment : IEnumerable<DataTableClassSegment>, IDisposable
    {
        private readonly DataTableClassSegment[] _classBuffer;
        private readonly DataTableFieldSegment[] _fieldBuffer;

        /// <summary>
        /// Sum 
        /// of the sizes 
        /// of the table 
        /// headers.
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
        public static int Dimensions
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
        public static int Flags
        {
            get => default;
        }

        /// <summary>
        /// A data type used for
        /// identifying internal 
        /// objects.
        /// </summary>
        public static uint Oid
        {
            get => default;
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
        public static int LowerBound
        {
            get => 1;
        }

        /// <inheritdoc/>
        public IEnumerator<DataTableClassSegment> GetEnumerator()
        {
            return new ArraySegment<DataTableClassSegment>(_classBuffer, default, Length).GetEnumerator();
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

        public DataTableSegment(PgSerializerOptions options, DataTable dt)
        {
            Length = dt.Rows.Count;

            _classBuffer = ArrayPool<DataTableClassSegment>.Shared.Rent(Length);
            _fieldBuffer = ArrayPool<DataTableFieldSegment>.Shared.Rent(Length * dt.Columns.Count);

            for (int i = 0; i < Length; i++)
            {
                _classBuffer[i] = new DataTableClassSegment(new ArraySegment<DataTableFieldSegment>(_fieldBuffer, i * dt.Columns.Count, dt.Columns.Count), options, dt.Rows[i]);

                SizeOverall += _classBuffer[i].SizeOverall;
            }

            SizeOverall += SizeHeaders;
        }
    }
}