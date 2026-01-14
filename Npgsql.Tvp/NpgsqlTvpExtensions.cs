using Npgsql.Tvp.Internal.Resolvers;

using Npgsql.TypeMapping;

namespace Npgsql.Tvp
{
    public static class NpgsqlTvpExtensions
    {
        /// <summary>
        /// Sets up the TVP plugin.
        /// </summary>
        public static TMapper UseTvp<TMapper>(this TMapper mapper) where TMapper : INpgsqlTypeMapper
        {
            mapper.AddTypeInfoResolverFactory(new TypeInfoResolverFactory());

            return mapper;
        }
    }
}