using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Abstracts;

using Npgsql.Tvp.Internal.Converters.Models;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System.Data.Common;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DRConverter(PgSerializerOptions options) : AbstractConverter<DbDataReader>
    {
        /// <inheritdoc/>
        protected override IParameter GetParameter(DbDataReader value)
        {
            return new ParameterDR(value, value.GetArrayType(options).Oid.Value, options);
        }
    }
}