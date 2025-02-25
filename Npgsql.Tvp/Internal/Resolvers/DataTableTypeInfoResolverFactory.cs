using Npgsql.Internal;

using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataTableTypeInfoResolverFactory<TTable> : PgTypeInfoResolverFactory where TTable : DataTable
    {
        /// <inheritdoc/>
        public override IPgTypeInfoResolver CreateResolver()
        {
            return new DataTableTypeInfoResolver<TTable>();
        }

        /// <inheritdoc/>
        public override IPgTypeInfoResolver CreateArrayResolver()
        {
            return default;
        }
    }
}