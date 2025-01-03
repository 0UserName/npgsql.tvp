using Npgsql.Internal;
using Npgsql.Internal.Postgres;
using Npgsql.PostgresTypes;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Npgsql.Tvp.Internal.Packets
{
    internal sealed class DataTablePacket : IEnumerable
    {
        ///<summary>
        /// Size of the segment 
        /// in which the OID is 
        /// transmitted.
        /// </summary>
        /// 
        /// <remarks>
        /// More details: <seealso cref="IPacket.Oid"/>
        /// </remarks>
        private const int HEADER_SIZE_OID = sizeof(uint);

        /// <summary>
        /// Size of the segment in 
        /// which the type size is 
        /// passed.
        /// </summary>
        private const int HEADER_SIZE_TYPE = sizeof(int);

        /// <summary>
        /// 
        /// </summary>
        private readonly PostgresCompositeType _type;

        /// <summary>
        /// 
        /// </summary>
        private readonly PgSerializerOptions _options;

        /// <summary>
        /// Columns equivalent to
        /// fields of a composite 
        /// type object.
        /// </summary>
        private readonly DataColumnCollection _columns;

        /// <summary>
        /// Row containing an array 
        /// of values equivalent to 
        /// the field values of a 
        /// composite type object.
        /// </summary>
        private readonly DataRow _row;

        /// <inheritdoc/>
        public int SizeHeaders
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SizePayload
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public int SizeOverall
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public uint Oid
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetSizeAsObject")]
        private extern static Size GetSize(PgConverter converter, SizeContext context, object value, ref object? writeState);

        /// <summary>
        /// 
        /// </summary>
        private int CalculateSizeHeaders()
        {
            return sizeof(int) + _columns.Count * (HEADER_SIZE_OID + HEADER_SIZE_TYPE);
        }

        /// <summary>
        /// 
        /// </summary>
        private int CalculateSizeOverall()
        {
            int size = SizeHeaders;

            for (int i = 0; i < _columns.Count; i++)
            {
                size += CalculateSizePayload(i, _row[i]);
            }

            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CalculateSizePayload(int index, object value)
        {
            int size;

            if (_columns[index].DataType.IsValueType)
            {
                size = Marshal.SizeOf(value);
            }
            else
            {
                object state = default;

                size = GetSize(GetConverter(index), default, value, ref state).Value;
            }

            return size;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _row.ItemArray.GetEnumerator();
        }

        public PgConverter GetConverter(int column)
        {
            PgTypeId typeId = new
            PgTypeId
            (_type.Fields[column].Type.OID);

            PgTypeInfo typeInfo = _options.GetDefaultTypeInfo(typeId);

            return typeInfo.GetObjectResolution(_row[_columns[column]]).Converter;
        }


        public DataTablePacket(PostgresCompositeType type, PgSerializerOptions options, DataColumnCollection columns, DataRow row)
        {
            _type = type;

            _options = options;

            _columns = columns;

            _row = row;

            SizeHeaders = CalculateSizeHeaders();
            SizeOverall = CalculateSizeOverall();

            SizePayload = SizeOverall - SizeHeaders;
        }
    }
}