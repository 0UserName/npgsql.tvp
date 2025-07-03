using Npgsql.Internal;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object item, PgConverterResolution resolution, Size writeRequirement, int size, object state)
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
            get => resolution.PgTypeId.Oid.Value;
        }

        public PgConverter Converter
        {
            get => resolution.Converter;
        }

        public Size WriteRequirement
        {
            get => writeRequirement;
        }

        /// <summary>
        /// Size of the item.
        /// </summary
        public int Size
        {
            get => size;
        }

        public object State
        {
            get => state;
        }
    }
}