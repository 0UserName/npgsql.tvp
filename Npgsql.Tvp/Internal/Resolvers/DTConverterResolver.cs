using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Models;

using Npgsql.Tvp.Internal.Resolvers.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DTConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DataTable>
    {
        private readonly
            DTConverter _converter = new
            DTConverter
            (options);

        /// <inheritdoc/>
        protected override PgTypeId GetArrayType(DataTable value)
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