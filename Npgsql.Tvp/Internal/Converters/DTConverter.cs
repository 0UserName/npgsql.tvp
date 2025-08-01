﻿using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Abstract;

using Npgsql.Tvp.Internal.Converters.Models;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DTConverter(PgSerializerOptions options) : AbstractConverter<DataTable>
    {
        /// <inheritdoc/>
        protected override IParameter GetParameter(DataTable value)
        {
            return new ParameterDT(value, options);
        }
    }
}