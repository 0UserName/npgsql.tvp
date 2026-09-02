using System;
using System.Data;

namespace Npgsql.Tvp.Tests.Rows
{
    internal sealed class DataTableReaderWrapper(int rows, DataTableReader reader) : IDataReader
    {
        /// <inheritdoc/>
        public object this[int i]
        {
            get => reader[i];
        }

        /// <inheritdoc/>
        public object this[string name]
        {
            get => reader[name];
        }

        /// <inheritdoc/>
        public int Depth
        {
            get => reader.Depth;
        }

        /// <inheritdoc/>
        public bool IsClosed
        {
            get => reader.IsClosed;
        }

        /// <inheritdoc/>
        public int RecordsAffected
        {
            get => rows;
        }

        /// <inheritdoc/>
        public int FieldCount
        {
            get => reader.FieldCount;
        }

        /// <inheritdoc/>
        public void Close()
        {
            reader.Close();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            reader.DisposeAsync();
        }

        /// <inheritdoc/>
        public bool GetBoolean(int i)
        {
            return reader.GetBoolean(i);
        }

        /// <inheritdoc/>
        public byte GetByte(int i)
        {
            return reader.GetByte(i);
        }

        /// <inheritdoc/>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>
        public char GetChar(int i)
        {
            return reader.GetChar(i);
        }

        /// <inheritdoc/>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <inheritdoc/>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetDataTypeName(int i)
        {
            return reader.GetDataTypeName(i);
        }

        /// <inheritdoc/>
        public DateTime GetDateTime(int i)
        {
            return reader.GetDateTime(i);
        }

        /// <inheritdoc/>
        public decimal GetDecimal(int i)
        {
            return reader.GetDecimal(i);
        }

        /// <inheritdoc/>
        public double GetDouble(int i)
        {
            return reader.GetDouble(i);
        }

        /// <inheritdoc/>
        public Type GetFieldType(int i)
        {
            return reader.GetFieldType(i);
        }

        /// <inheritdoc/>
        public float GetFloat(int i)
        {
            return reader.GetFloat(i);
        }

        /// <inheritdoc/>
        public Guid GetGuid(int i)
        {
            return reader.GetGuid(i);
        }

        /// <inheritdoc/>
        public short GetInt16(int i)
        {
            return reader.GetInt16(i);
        }

        /// <inheritdoc/>
        public int GetInt32(int i)
        {
            return reader.GetInt32(i);
        }

        /// <inheritdoc/>
        public long GetInt64(int i)
        {
            return reader.GetInt64(i);
        }

        /// <inheritdoc/>
        public string GetName(int i)
        {
            return reader.GetName(i);
        }

        /// <inheritdoc/>
        public int GetOrdinal(string name)
        {
            return reader.GetOrdinal(name);
        }

        /// <inheritdoc/>
        public DataTable GetSchemaTable()
        {
            return reader.GetSchemaTable();
        }

        /// <inheritdoc/>
        public string GetString(int i)
        {
            return reader.GetString(i);
        }

        /// <inheritdoc/>
        public object GetValue(int i)
        {
            return reader.GetValue(i);
        }

        /// <inheritdoc/>
        public int GetValues(object[] values)
        {
            return reader.GetValues(values);
        }

        /// <inheritdoc/>
        public bool IsDBNull(int i)
        {
            return reader.IsDBNull(i);
        }

        /// <inheritdoc/>
        public bool NextResult()
        {
            return reader.NextResult();
        }

        /// <inheritdoc/>
        public bool Read()
        {
            return reader.Read();
        }
    }
}