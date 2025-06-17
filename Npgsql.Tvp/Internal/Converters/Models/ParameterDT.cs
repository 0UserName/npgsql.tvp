using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstract;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDT(DataTable value, PgSerializerOptions options) : AbstractParameter(value.TableName, options)
    {
        /// <inheritdoc/>
        public override int ColumnsCount
        {
            get => value.Columns.Count;
        }

        /// <inheritdoc/>
        public override int RowsCount
        {
            get => value.Rows.Count;
        }

        /// <inheritdoc/>
        public override Value this[int row, int column]
        {
            get => CreateValue(value.Rows[row][column], value.Columns[column].DataType);
        }
    }
}