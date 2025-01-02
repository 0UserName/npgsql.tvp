using Npgsql.Internal;
using Npgsql.PostgresTypes;

using Npgsql.Tvp.Internal.Packets;

using System;
using System.Data;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Convertors
{
    internal sealed class DataTableConverter<TTable> : PgStreamingConverter<TTable> where TTable : DataTable
    {
        private readonly PostgresCompositeType _type;

        /// <inheritdoc/>
        public override TTable Read(PgReader reader)
        {
            throw new NotImplementedException($"{ nameof(DataTable) } is not supported due to lack of need");
        }

        /// <inheritdoc/>
        public override ValueTask<TTable> ReadAsync(PgReader reader, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException($"{ nameof(DataTable) } is not supported due to lack of need");
        }

        /// <inheritdoc/>
        public override Size GetSize(SizeContext context, TTable value, ref object writeState)
        {
            DataTableArrayPacket packet = new
            DataTableArrayPacket
            (value, _type);

            writeState = packet;

            return packet.SizeTotal;
        }

        /// <inheritdoc/>
        public override void Write(PgWriter writer, TTable value)
        {
            WriteAsync(writer, value).AsTask().Wait();
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(PgWriter writer, TTable value, CancellationToken cancellationToken = default)
        {
            return DataTableWriter.WriteAsync(writer, cancellationToken);
        }

        public DataTableConverter(PostgresCompositeType type)
        {
            _type = type;
        }
    }
}