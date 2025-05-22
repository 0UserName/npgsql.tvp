using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Resolvers.Abstract;

using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DTConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DataTable>(options)
    {
        private DTConverter _converter;

        /// <inheritdoc/>
        protected override string GetDataTypeName(DataTable value)
        {
            return value.TableName;
        }

        /// <inheritdoc/>
        protected override PgConverter GetConverter(PgSerializerOptions options)
        {
            return _converter ??= new DTConverter(options);
        }
    }
}