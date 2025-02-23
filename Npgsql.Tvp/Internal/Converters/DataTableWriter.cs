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

            writer.WriteInt32(Table.DIMENSIONS);
            writer.WriteInt32(Table.FLAGS);
            writer.WriteUInt32(table.Oid);
            writer.WriteInt32(table.Value.Rows.Count);
            writer.WriteInt32(Table.LOWER_BOUND);

            for (int i = 0; i < table.Value.Rows.Count; i++)
            {
                if (writer.ShouldFlush(Field.HEADERS_SIZE)) // TODO: перепроверить, т.к для строки (композита) могут быть другие значения.
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                }

                writer.WriteInt32(table.GetRowSize(i));
                writer.WriteInt32(table.Value.Columns.Count);

                for (int j = 0; j < table.Value.Columns.Count; j++)
                {
                    Field column = table[i, j];

                    if (writer.ShouldFlush(Field.HEADERS_SIZE))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }

                    writer.WriteUInt32(column.Oid);
                    writer.WriteInt32(column.Size);

                    if (!column.IsDbNull())
                    {
                        if (writer.ShouldFlush(column.Size))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                        }

                        // None of the converters except CompositeConverter<T> use state, so we leave it empty.
                        using (await writer.BeginNestedWriteAsync(column.GetBufferRequirements().Write, column.Size, default, cancellationToken))
                        {
                            // Binary representation of element.
                            await _PgConverter.WriteAsObjectAsync(column.GetConverter(), writer, column.Value, cancellationToken).ConfigureAwait(default);
                        }
                    }
                }
            }
        }
    }
}