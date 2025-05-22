using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;
using System.Data;
using System.Data.Common;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class TypeInfoResolver : IPgTypeInfoResolver
    {
        /// <inheritdoc/>
        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            switch (type)
            {
                case Type t when t.IsAssignableTo(typeof(DataTable   )): return new PgResolverTypeInfo(options, new DTConverterResolver(options), default);
                case Type t when t.IsAssignableTo(typeof(DbDataReader)): return new PgResolverTypeInfo(options, new DRConverterResolver(options), default);

                default: return default;
            }
        }
    }
}