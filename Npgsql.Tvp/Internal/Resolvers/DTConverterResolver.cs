using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Models;

using Npgsql.Tvp.Internal.Resolvers.Abstract;

using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DTConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DataTable>(options)
    {
        private readonly
            DTConverter _converter = new
            DTConverter
            (options);

        /// <inheritdoc/>
        protected override string GetDataTypeName(DataTable value)
        {
            return value.GetDataTypeName();
        }

        /// <inheritdoc/>
        protected override PgConverter GetConverter(PgSerializerOptions options)
        {
            return _converter;
        }
    }
}