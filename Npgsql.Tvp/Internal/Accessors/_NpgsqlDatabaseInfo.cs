using Npgsql.Internal;

using System.Runtime.CompilerServices;

namespace Npgsql.Tvp.Internal.Accessors
{
    internal static class _NpgsqlDatabaseInfo
    {
        /// <summary>
        /// Gets 
        /// an instance of <see cref="NpgsqlDatabaseInfo"/> 
        /// that provides information about PostgreSQL and 
        /// PostgreSQL-compatible databases.
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_DatabaseInfo")]
        public extern static NpgsqlDatabaseInfo Get(PgSerializerOptions options);
    }
}