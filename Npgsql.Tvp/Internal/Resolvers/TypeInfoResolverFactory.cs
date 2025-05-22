using Npgsql.Internal;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class TypeInfoResolverFactory : PgTypeInfoResolverFactory
    {
        /// <inheritdoc/>
        public override IPgTypeInfoResolver CreateResolver()
        {
            return new TypeInfoResolver();
        }

        /// <inheritdoc/>
        public override IPgTypeInfoResolver CreateArrayResolver()
        {
            return default;
        }
    }
}