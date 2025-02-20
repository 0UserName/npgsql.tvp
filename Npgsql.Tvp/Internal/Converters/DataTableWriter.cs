using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;
using Npgsql.Tvp.Internal.Converters.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class DataTableWriter
    {
        /// <summary>
        /// 
        /// </summary>
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            Table table = writer.Current.WriteState as Table;

            if (writer.ShouldFlush(table.HeadersSize))
            {
                await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
            }

            // 4 bytes with number of dimensions.
            writer.WriteInt32(Table.DIMENSIONS);

            // 4 bytes, boolean
            // indicating nulls
            // present or not.
            writer.WriteInt32(Table.FLAGS);

            // 4 bytes array type OID.
            writer.WriteUInt32(Table.Oid);

            // 4 bytes for length.
            writer.WriteInt32(table.ClassLength);

            // 4 bytes for lower bound on length
            // to check for overflow (it appears
            // this value can always be 0).
            writer.WriteInt32(Table.LOWER_BOUND);

            using (table)
            {
                for (int r = 0; r < table.ClassLength; r++)
                {
                    Class row = table[r];

                    if (writer.ShouldFlush(Field.HEADERS_SIZE))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }

                    writer
                        .WriteInt32(row.Size);
                    writer
                        .WriteInt32(table.FieldLength);


                    for (int c = 0; c < table.FieldLength; c++)
                    {
                        Field column = row[c];

                        if (writer.ShouldFlush(Field.HEADERS_SIZE))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                        }

                        // 4 bytes elemen type OID.
                        writer.WriteUInt32(column.Oid);

                        // 4 bytes describing length
                        // of element, -1 means null.
                        writer.WriteInt32(column.ValueSize);

                        if (!column.IsDbNull())
                        {
                            if (writer.ShouldFlush(column.ValueSize))
                            {
                                await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                            }

                            // Binary representation of element.
                            await _PgConverter.WriteAsObjectAsync(column.GetConverter(), writer, column.Value, cancellationToken).ConfigureAwait(default);
                        }
                    }
                }
            }
        }
    }
}