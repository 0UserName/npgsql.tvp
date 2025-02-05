using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataTableTypeInfoResolver<TTable> : IPgTypeInfoResolver where TTable : DataTable
    {
        /// <inheritdoc/>
        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            return type == typeof(TTable) ? new PgResolverTypeInfo(options, new DataTableConverterResolver<TTable>(options), default) : default;
        }
    }
}