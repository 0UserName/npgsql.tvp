using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Packets;

using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class DataTableWriter
    {
        /// <summary>
        /// Extension to <paramref name="writer"/> call chain 
        /// without the need to specify the type of the value 
        /// being written.
        /// </summary>
        private static PgWriter WriteHeader<TValue>(this PgWriter writer, TValue value) where TValue : struct
        {
            switch (value)
            {
                case int v:
                    writer.WriteInt32(v);
                    break;
                case uint v:
                    writer.WriteUInt32(v);
                    break;
            }

            return writer;
        }

        /// <summary>
        /// 
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "WriteAsObject")]
        private extern static ValueTask WriteValue(PgConverter converter, bool async, PgWriter writer, object value, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            using (DataTableArrayPacket arrayPacket = writer.Current.WriteState as DataTableArrayPacket)
            {
                if (writer.ShouldFlush(arrayPacket.SizeHeaders))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                }

                writer
                    .WriteHeader(arrayPacket.HeaderArrayDimensions)
                    .WriteHeader(arrayPacket.HeaderArrayFlags)
                    .WriteHeader(arrayPacket.HeaderArrayElementOid)
                    .WriteHeader(arrayPacket.HeaderArrayLenght)
                    .WriteHeader(arrayPacket.HeaderArrayLowerbound);

                foreach (DataTablePacket packet in arrayPacket)
                {
                    writer
                        .WriteHeader(packet.SizeOverall)
                        .WriteHeader(4);

                    for (int j = 0; j < 4; j++)
                    {
                        writer
                            .WriteHeader(arrayPacket[j].Oid)
                            .WriteHeader(arrayPacket[i].SizePayload);
                        //  .Write(arrayPacket[i][j]); write value with specific convertor
                    }
                }
            }
        }
    }
}