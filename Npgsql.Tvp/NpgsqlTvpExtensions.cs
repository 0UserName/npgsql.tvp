using Npgsql.Tvp.Internal.Resolvers;

using Npgsql.TypeMapping;

namespace Npgsql.Tvp
{
    /// <summary>
    /// Extension that adds the 
    /// TVP plugin to an Npgsql 
    /// type mapper.
    /// </summary>
    public static class NpgsqlTvpExtensions
    {
        /// <summary>
        /// Sets up TVP mappings for PostgreSQL composite type arrays.
        /// </summary>
        public static TMapper UseTvp<TMapper>(this TMapper mapper) where TMapper : INpgsqlTypeMapper
        {
            mapper.AddTypeInfoResolverFactory(new TypeInfoResolverFactory());

            return mapper;
        }
    }
}