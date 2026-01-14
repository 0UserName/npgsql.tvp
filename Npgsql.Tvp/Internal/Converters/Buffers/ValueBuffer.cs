using Npgsql.Tvp.Internal.Converters.Models;

using System;

namespace Npgsql.Tvp.Internal.Converters.Buffers
{
    internal sealed class ValueBuffer(int columnsCount, int rowsCount) : IDisposable
    {
        private int _written = 0;

        private int _rSize = 0;
        private int _tSize = 0;

        /// <summary>
        /// The row values.
        /// </summary>
        private Value[] _items = ArrayPoolExtensions.Rent<Value>(columnsCount * rowsCount);

        /// <summary>
        /// The sizes of the rows.
        /// </summary>
        private int[] _sizes = ArrayPoolExtensions.Rent<int>(rowsCount);

        /// <summary>
        /// The amount 
        /// of values written to the 
        /// underlying buffer so far.
        /// </summary>
        public int Written
        {
            get => _written;
        }

        /// <summary>
        /// The total size of all rows.
        /// </summary>
        public int Size
        {
            get => _tSize;
        }

        /// <summary>
        /// Gets the size of the specified row.
        /// </summary>
        public int this[int row]
        {
            get => _sizes[row];
        }

        /// <summary>
        /// Gets the value at 
        /// the specified row 
        /// and column.
        /// </summary>
        public Value this[int row, int column]
        {
            get => _items[columnsCount * row + column];
        }

        /// <summary>
        /// Returns 
        /// true if the end of 
        /// the row is reached.
        /// </summary>
        private bool ShouldFlushSize()
        {
            return _written % columnsCount == 0;
        }

        /// <summary>
        /// Writes the row size to 
        /// the buffer and updates
        /// the total size.
        /// </summary>
        private void FlushSize()
        {
            _tSize += _rSize += sizeof(int) + Value.METADATA_SIZE * columnsCount;

            _sizes[_written / columnsCount - 1] = _rSize;

            _rSize = 0;
        }

        /// <inheritdoc cref="FlushSize"/>
        /// 
        /// <param name="size">
        /// -1 indicates a NULL column value.
        /// </param>
        private void WriteSize(int size)
        {
            if (size != Constants.NULL_SIZE)
            {
                _rSize += size;
            }

            if (ShouldFlushSize())
            {
                FlushSize();
            }
        }

        /// <summary>
        /// Writes the value to the buffer.
        /// </summary>
        public void Write(Value value)
        {
            if (_written == _items.Length)
            {
                _items = _items.Resize(true);
                _sizes = _sizes.Resize();
            }

            _items[_written++] = value;

            WriteSize(value.BufferRequirement.Value);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _items.Return(0, _written);
            _sizes.Return();
        }
    }
}