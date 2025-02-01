using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;
using Npgsql.Tvp.Internal.Segments;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class DataTableWriter
    {
        /// <summary>
        /// 
        /// </summary>
        private static async ValueTask WriteAsync(this PgWriter writer, DataTableFieldSegment column, CancellationToken cancellationToken)
        {
            if (!column.IsNull)
            {
                using (NestedWriteScope scope = await writer.BeginNestedWriteAsync(default, column.SizePayload, column.WriteState, cancellationToken))
                {
                    await _PgConverter.WriteValue(column.Converter, true, writer, column.Payload, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Asynchronously writes 
        /// table segments to the 
        /// underlying stream.
        /// </summary>
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            DataTableSegment table = writer.Current.WriteState as DataTableSegment;

            writer
                .WriteInt32(table.Dimensions);
            writer
                .WriteInt32(table.Flags);
            writer
                .WriteUInt32(table.Oid);
            writer
                .WriteInt32(table.Length);
            writer
                .WriteInt32(table.LowerBound);

            foreach (DataTableClassSegment row in table)
            {
                writer
                    .WriteInt32(row.SizeOverall);
                writer
                    .WriteInt32(row.Length);

                foreach (DataTableFieldSegment column in row)
                {
                    writer
                        .WriteUInt32(column.Oid);
                    writer
                        .WriteInt32(column.SizePayload);

                    await writer.WriteAsync(column, cancellationToken);
                }
            }
        }
    }
}