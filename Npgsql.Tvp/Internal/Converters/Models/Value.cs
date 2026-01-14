using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object value, uint oid, PgConverter converter, Size writeRequirement)
    {
        private readonly (Size BR, object WS) _state = GetSizeOrDbNullAsObject(value, converter, writeRequirement);

        /// <summary>
        /// Size of the target value headers.
        /// </summary
        /// 
        /// <remarks>
        /// OID + Size.
        /// </remarks>
        public const int METADATA_SIZE = sizeof(uint) + sizeof(int);

        public object UnderlyingValue
        {
            get => value;
        }

        /// <summary>
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </summary>
        public uint OID
        {
            get => oid;
        }

        public PgConverter Converter
        {
            get => converter;
        }

        public Size BufferRequirement
        {
            get => _state.BR;
        }

        /// <summary>
        /// Not really used.
        /// </summary>
        public object WriteState
        {
            get => _state.WS;
        }

        /// <remarks>
        /// GetDefaultTypeInfo returns a 
        /// non-nullable converter because DataTable 
        /// and DbDataReader do not support nullable 
        /// column types.
        /// </remarks>
        private static (Size, object) GetSizeOrDbNullAsObject(object item, PgConverter converter, Size writeRequirement)
        {
            object writeState = default;

            return (item is DBNull or null ? Constants.NULL_SIZE : Accessors.GetSizeOrDbNullAsObject(default, converter, DataFormat.Binary, writeRequirement, item, ref writeState).Value, writeState);
        }
    }
}