using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Models;

using Npgsql.Tvp.Internal.Resolvers.Abstract;

using System.Data.Common;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DRConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DbDataReader>(options)
    {
        private readonly
            DRConverter _converter = new
            DRConverter
            (options);

        /// <inheritdoc/>
        protected override string GetDataTypeName(DbDataReader value)
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