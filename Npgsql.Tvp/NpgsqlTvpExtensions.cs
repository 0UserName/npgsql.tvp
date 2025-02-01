using Npgsql.Tvp.Internal.Resolvers;

using Npgsql.TypeMapping;

using System.Data;

namespace Npgsql.Tvp
{
    /// <summary>
    /// Extension adding the TVP
    /// plugin to an Npgsql type 
    /// mapper.
    /// </summary>
    public static class NpgsqlTvpExtensions
    {
        /// <summary>
        /// Sets up TVP mappings for
        /// the PostgreSQL composite 
        /// type arrays.
        /// </summary>
        /// 
        /// <param name="mapper">
        /// The type mapper to set up (global or connection-specific).
        /// </param>
        public static INpgsqlTypeMapper UseTvp<TTable>(this INpgsqlTypeMapper mapper) where TTable : DataTable
        {
            mapper.AddTypeInfoResolverFactory(new DataTableTypeInfoResolverFactory<TTable>());

            return mapper;
        }

        /// <inheritdoc cref="UseTvp(INpgsqlTypeMapper)"/>.
        public static TMapper UseTvp<TMapper, TTable>(this TMapper mapper) where TMapper : INpgsqlTypeMapper where TTable : DataTable
        {
            mapper.AddTypeInfoResolverFactory(new DataTableTypeInfoResolverFactory<TTable>());

            return mapper;
        }
    }
}