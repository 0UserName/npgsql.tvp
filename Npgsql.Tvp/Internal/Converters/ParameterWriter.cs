using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Binders;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class ParameterWriter
    {
        /// <summary>
        /// Writes the column value to the driver buffer.
        /// </summary>
        /// 
        /// <remarks>
        /// Called only through an expression-built lambda, not invoked directly.
        /// </remarks>
        public static ValueTask WriteValueAsync<T>(PgConverter converter, PgWriter writer, IStrongBox box, CancellationToken cancellationToken = default)
        {
            return ((PgConverter<T>)converter).WriteAsync(writer, ((StrongBox<T>)box).Value, cancellationToken);
        }

        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            using (IParameter parameter = (IParameter)writer.Current.WriteState)
            {
                int columns = parameter.Columns;
                int rows = parameter.Rows;

                if (writer.ShouldFlush(parameter.MetadataSize))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                }

                writer.WriteInt32(Constants.DIMENSIONS);
                writer.WriteInt32(Constants.FLAGS);
                writer.WriteUInt32(parameter.OID);
                writer.WriteInt32(rows);
                writer.WriteInt32(Constants.LOWER_BOUND);

                for (int i = 0; i < rows; i++)
                {
                    if (writer.ShouldFlush(sizeof(int) + sizeof(int)))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                    }

                    writer.WriteInt32(parameter[i]);
                    writer.WriteInt32(columns);

                    for (int j = 0; j < columns; j++)
                    {
                        if (writer.ShouldFlush(sizeof(uint) + sizeof(int)))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                        }

                        ref Writable writable = ref parameter.GetMoveNext();

                        writable.Deconstruct(out uint oid, out int size, out IStrongBox box, out PgConverter converter, out object writeState);

                        writer.WriteUInt32(oid);
                        writer.WriteInt32(size);

                        if (size != Constants.NULL_SIZE)
                        {
                            using (await writer.BeginNestedWriteAsync(size, size, writeState, cancellationToken).ConfigureAwait(false))
                            {
                                await WritableBinder.Get(oid)(converter, writer, box, cancellationToken).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }
    }
}