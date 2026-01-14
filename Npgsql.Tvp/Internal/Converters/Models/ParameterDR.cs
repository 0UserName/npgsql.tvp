using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstracts;

using System.Data.Common;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDR(DbDataReader value, uint oid, PgSerializerOptions options) : AbstractParameter(value.FieldCount, 32, oid, options)
    {
        /// <inheritdoc/>
        protected override void FillBuffer()
        {
            while (value.Read())
            {
                for (int i = 0; i < value.FieldCount; i++)
                {
                    Buffer.Write(CreateValue(value.GetValue(i), value.GetFieldType(i)));
                }
            }
        }
    }
}