namespace Npgsql.Tvp.Internal.Segments.Abstract
{
    internal abstract class AbstractTyped(uint oid)
    {
        /// <summary>
        /// Object identifiers (OIDs) are used internally by PostgreSQL as 
        /// primary keys for various system tables. Type oid represents an 
        /// object identifier.
        /// </summary>
        public uint Oid
        {
            get => oid;
        }
    }
}