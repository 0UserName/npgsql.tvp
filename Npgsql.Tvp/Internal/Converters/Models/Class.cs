using Npgsql.Internal;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Class(ArraySegment<Field> fields, DataRow row, PgSerializerOptions options)
    {
        /// <summary>
        /// Row size.
        /// </summary>
        public int Size
        {
            get => GetSize(fields, row, options);
        }

        /// <summary>
        /// Gets 
        /// the column stored in 
        /// the row specified by 
        /// index.
        /// </summary>
        public Field this[int index]
        {
            get => fields[index];
        }

        private static int GetSize(ArraySegment<Field> fields, DataRow row, PgSerializerOptions options)
        {
            int size = sizeof(int) + fields.Count * Field.HEADERS_SIZE;

            for (int i = 0; i < fields.Count; i++)
            {
                size += Math.Max(default, (fields[i] = new Field(row[row.Table.Columns[i]], options.GetDefaultTypeInfo(row.Table.Columns[i].DataType))).ValueSize);
            }

            return size;
        }
    }
}