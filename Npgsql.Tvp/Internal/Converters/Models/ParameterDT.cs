using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDT(DataTable value, uint oid, PgSerializerOptions options) : AbstractParameter(value.Columns.Count, value.Rows.Count, oid, options)
    {
        /// <inheritdoc/>
        protected override void FillBuffer()
        {
            for (int i = 0; i < ColumnsCount * value.Rows.Count; i++)
            {
                int rIndex = i / ColumnsCount;
                int cIndex = i % ColumnsCount;

                Buffer.Write(CreateValue(value.Rows[rIndex][cIndex], value.Columns[cIndex].DataType));
            }
        }
    }
}