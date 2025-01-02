namespace Npgsql.Tvp.Internal.Packets.Contracts
{
    internal interface IPacket
    {
        /// <summary>
        /// Size of headers and payload.
        /// </summary>
        int SizeOverall
        {
            get;
        }

        /// <summary>
        /// Size of headers.
        /// </summary>
        int SizeHeaders
        {
            get;
        }

        /// <summary>
        /// Object identifiers (OIDs) are used internally by PostgreSQL as 
        /// primary keys for various system tables. Type oid represents an 
        /// object identifier.
        /// </summary>
        uint Oid
        {
            get;
        }
    }
}