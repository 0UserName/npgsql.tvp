using Npgsql.Internal;

using System;
using System.Buffers;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class Table : IDisposable
    {
        private readonly Class[] _classBuffer;
        private readonly Field[] _fieldBuffer;

        /// <summary>
        /// Number of dimensions.
        /// </summary>
        public const int DIMENSIONS = 1;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Not really used.
        /// </remarks>
        public const int FLAGS = default;

        /// <summary>
        /// Index of the first row.
        /// </summary>
        public const int LOWER_BOUND = default;

        /// <summary>
        /// Sum 
        /// of the sizes 
        /// of the table 
        /// headers.
        /// </summary>
        /// 
        /// <remarks>
        /// See also: <seealso cref="TotalSize"/>.
        /// </remarks>
        public int HeadersSize
        {
            get => sizeof(int) + sizeof(int) + sizeof(uint) + DIMENSIONS * (sizeof(int) + sizeof(int)) + sizeof(int) * ClassLength;
        }

        /// <summary>
        /// Sum of 
        /// the headers 
        /// and payload 
        /// sizes. 
        /// </summary>
        public int TotalSize
        {
            get;
            private set;
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
        public int ClassLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Total number of columns.
        /// </summary>
        public int FieldLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the row stored in 
        /// the table specified by 
        /// index.
        /// </summary>
        public Class this[int index]
        {
            get => _classBuffer[index];
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ArrayPool<Class>.Shared.Return(_classBuffer, true);
            ArrayPool<Field>.Shared.Return(_fieldBuffer, true);
        }

        public Table(DataTable value, PgSerializerOptions options)
        {
            ClassLength = value.Rows.Count;
            FieldLength = value.Columns.Count;

            _classBuffer = ArrayPool<Class>.Shared.Rent(ClassLength);
            _fieldBuffer = ArrayPool<Field>.Shared.Rent(ClassLength * value.Columns.Count);

            for (int i = 0; i < ClassLength; i++)
            {
                _classBuffer[i] = new Class(new ArraySegment<Field>(_fieldBuffer, i * value.Columns.Count, value.Columns.Count), value.Rows[i], options);

                TotalSize += _classBuffer[i].Size;
            }

            TotalSize += HeadersSize;
        }
    }
}