using Npgsql.Internal;
using Npgsql.PostgresTypes;

using Npgsql.Tvp.Internal.Packets;

using System;
using System.Data;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DataTableConverter<TTable> : PgStreamingConverter<TTable> where TTable : DataTable
    {
        /// <summary>
        /// Composite type 
        /// definition that the table 
        /// structure must conform to.
        /// </summary>
        private readonly PostgresCompositeType _type;

        /// <summary>
        /// 
        /// </summary>
        private readonly PgSerializerOptions _options;

        /// <inheritdoc/>
        public override TTable Read(PgReader reader)
        {
            return ReadAsync(reader).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public override ValueTask<TTable> ReadAsync(PgReader reader, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException($"{ nameof(DataTable) } is not supported");
        }

        /// <inheritdoc/>
        public override Size GetSize(SizeContext context, TTable value, ref object writeState)
        {
            DataTableArrayPacket packet = new
            DataTableArrayPacket
            (_type, _options, value);

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

        public DataTableConverter(PostgresCompositeType type, PgSerializerOptions options)
        {
            _type = type;

            _options = options;
        }
    }
}