using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class ParameterWriter
    {
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            using (IParameter parameter = (IParameter)writer.Current.WriteState)
            {
                int cCount = parameter.ColumnsCount;
                int rCount = parameter.RowsCount;

                if (writer.ShouldFlush(parameter.MetadataSize))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                }

                writer.WriteInt32(Constants.DIMENSIONS);
                writer.WriteInt32(Constants.FLAGS);
                writer.WriteUInt32(parameter.OID);
                writer.WriteInt32(rCount);
                writer.WriteInt32(Constants.LOWER_BOUND);

                for (int i = 0; i < rCount; i++)
                {
                    if (writer.ShouldFlush(sizeof(int) + sizeof(int)))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                    }

                    writer.WriteInt32(parameter[i]);
                    writer.WriteInt32(cCount);

                    for (int j = 0; j < cCount; j++)
                    {
                        if (writer.ShouldFlush(sizeof(uint) + sizeof(int)))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                        }

                        Value value = parameter[i, j];

                        int size = value.BufferRequirement.Value;

                        writer.WriteUInt32(value.OID);
                        writer.WriteInt32(size);

                        if (size != Constants.NULL_SIZE)
                        {
                            using (await writer.BeginNestedWriteAsync(value.BufferRequirement, size, value.WriteState, cancellationToken).ConfigureAwait(false))
                            {
                                await Accessors.WriteAsObjectAsync(value.Converter, writer, value.UnderlyingValue, cancellationToken).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }
    }
}