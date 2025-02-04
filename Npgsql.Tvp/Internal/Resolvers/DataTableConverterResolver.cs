using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal class DataTableConverterResolver<TTable>(PgSerializerOptions options) : PgConverterResolver<TTable> where TTable : DataTable
    {
        private DataTableConverter<TTable> _converter;

        /// <inheritdoc/>
        public override PgConverterResolution? Get(TTable value, PgTypeId? expectedPgTypeId)
        {
            return GetDefault(options.GetArrayTypeId(new DataTypeName(value.TableName ?? throw new InvalidOperationException("Table name cannot be empty"))));
        }

        /// <inheritdoc/>
        public override PgConverterResolution GetDefault(PgTypeId? pgTypeId)
        {
            return new PgConverterResolution(_converter ??= new DataTableConverter<TTable>(options), pgTypeId.Value);
        }
    }
}