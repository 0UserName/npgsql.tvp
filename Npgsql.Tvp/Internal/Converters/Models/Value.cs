using Npgsql.Internal;
using Npgsql.Internal.Postgres;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object item, Oid oid, PgConverter converter, Size writeRequirement, int size)
    {
        /// <summary>
        /// Size of the item headers.
        /// </summary
        /// 
        /// <remarks>
        /// OID + Length.
        /// </remarks>
        public const int METADATA_SIZE = sizeof(uint) + sizeof(int);

        public object Item
        {
            get => item;
        }

        /// <summary>
        /// The data type's OID - a unique id identifying 
        /// the data type in a given database (in pg_type).
        /// </summary>
        public uint OID
        {
            get => oid.Value;
        }

        public PgConverter Converter
        {
            get => converter;
        }

        public Size WriteRequirement
        {
            get => writeRequirement;
        }

        public int Size
        {
            get => size;
        }
    }
}