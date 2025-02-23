using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class Table(DataTable value, PgSerializerOptions options)
    {
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
        /// 
        /// </summary>
        public DataTable Value
        {
            get => value;
        }

        /// <summary>
        /// Sum 
        /// of the sizes 
        /// of the table 
        /// headers.
        /// </summary>
        /// 
        /// <remarks>
        /// See also: <seealso cref="Size"/>.
        /// </remarks>
        public int HeadersSize
        {
            get => sizeof(int) + sizeof(int) + sizeof(uint) + DIMENSIONS * (sizeof(int) + sizeof(int)) + sizeof(int) * Value.Rows.Count;
        }

        /// <summary>
        /// A data type used for
        /// identifying internal 
        /// objects.
        /// </summary>
        public uint Oid
        {
            get => options.GetArrayElementTypeId(new DataTypeName(Value.TableName).ToArrayName()).Oid.Value;
        }

        /// <summary>
        /// Gets the size of the table in bytes. 
        /// </summary>
        public int GetTableSize()
        {
            int size = HeadersSize;

            for (int i = 0; i < value.Rows.Count; i++)
            {
                size += GetRowSize(i);
            }

            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetRowSize(int row)
        {
            int size = sizeof(int) + Value.Columns.Count * Field.HEADERS_SIZE;

            for (int i = row * Value.Columns.Count; i < row * Value.Columns.Count + Value.Columns.Count; i++)
            {
                size += Math.Max(default, this[row, i].Size);
            }

            return size;
        }

        /// <summary>
        /// Gets the column stored 
        /// in the table specified 
        /// by index.
        /// </summary>
        public Field this[int row, int column]
        {
            get => new Field(Value.Rows[row][column], options.GetDefaultTypeInfo(Value.Columns[column].DataType));
        }
    }
}