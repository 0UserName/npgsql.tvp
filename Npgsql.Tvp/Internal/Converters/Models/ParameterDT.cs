using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstract;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDT(DataTable value, PgSerializerOptions options) : AbstractParameter(value.TableName, value.Columns.Count, value.Rows.Count, options)
    {
        /// <inheritdoc/>
        public override Value this[int row, int column]
        {
            get => CreateValue(value.Rows[row][column], value.Columns[column].DataType);
        }
    }
}