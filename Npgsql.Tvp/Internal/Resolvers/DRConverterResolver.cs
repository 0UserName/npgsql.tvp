using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Models;

using Npgsql.Tvp.Internal.Resolvers.Abstracts;

using System.Data.Common;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DRConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DbDataReader>
    {
        private readonly
            DRConverter _converter = new
            DRConverter
            (options);

        /// <inheritdoc/>
        protected override PgTypeId GetArrayType(DbDataReader value)
        {
            return value.GetArrayType(options);
        }

        /// <inheritdoc/>
        protected override PgConverter GetConverter()
        {
            return _converter;
        }
    }
}