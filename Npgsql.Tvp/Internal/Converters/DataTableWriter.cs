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
        /// Writes 
        /// the binary representation of DataTable: 
        /// set of rows is interpreted as an array 
        /// of type T, where T is a composite type 
        /// described by columns.
        /// </summary>
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            DataTableSegment table = writer.Current.WriteState as DataTableSegment;

            if (writer.ShouldFlush(table.SizeHeaders))
            {
                await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
            }

            // 4 bytes with number of dimensions.
            writer.WriteInt32(DataTableSegment.Dimensions);

            // 4 bytes, boolean
            // indicating nulls
            // present or not.
            writer.WriteInt32(DataTableSegment.Flags);

            // 4 bytes array type OID.
            writer.WriteUInt32(DataTableSegment.Oid);

            // 4 bytes for length.
            writer.WriteInt32(table.Length);

            // 4 bytes for lower bound on length
            // to check for overflow (it appears
            // this value can always be 0).
            writer.WriteInt32(DataTableSegment.LowerBound);


            foreach (DataTableClassSegment row in table)
            {
                if (writer.ShouldFlush(DataTableClassSegment.SizeHeaders))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                }

                writer
                    .WriteInt32(row.SizeOverall);
                writer
                    .WriteInt32(row.Length);


                foreach (DataTableFieldSegment column in row)
                {
                    if (writer.ShouldFlush(DataTableFieldSegment.SizeHeaders))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }

                    // 4 bytes elemen type OID.
                    writer.WriteUInt32(column.Oid);

                    // 4 bytes describing length
                    // of element, -1 means null.
                    writer.WriteInt32(column.SizePayload);

                    if (!column.IsNull)
                    {
                        using (await writer.BeginNestedWriteAsync(column.BufferRequirements.Write, column.SizePayload, column.WriteState, cancellationToken))
                        {
                            // Binary representation of element.
                            await _PgConverter.WriteValueAsync(column.Converter, writer, column.Payload, cancellationToken).ConfigureAwait(default);
                        }
                    }
                }
            }
        }
    }
}