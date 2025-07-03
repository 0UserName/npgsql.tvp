using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstract;

using System;
using System.Data.Common;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal class ParameterDR(DbDataReader value, PgSerializerOptions options) : AbstractParameter(value.GetDataTypeName(), value.FieldCount, default, options)
    {
        /// <inheritdoc/>
        public override int RowsCount
        {
            get => throw new NotImplementedException("Will be implemented in future versions");
        }

        /// <inheritdoc/>
        public override Value this[int row, int column]
        {
            get => throw new NotImplementedException("Will be implemented in future versions");
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            // TODO: Dispose the internal buffer.
        }
    }
}