using Npgsql.Internal;

using System;
using System.Data;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DataTableConverter<TTable>(PgSerializerOptions options) : PgStreamingConverter<TTable> where TTable : DataTable
    {
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
            DataTableSegment segment = new
            DataTableSegment
            (options, value);

            writeState = segment;

            return segment.SizeOverall;
        }

        /// <inheritdoc/>
        public override void Write(PgWriter writer, TTable value)
        {
            WriteAsync(writer, value).AsTask().Wait();
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(PgWriter writer, TTable value, CancellationToken cancellationToken = default)
        {
            return DataTableWriter.WriteAsync(writer, options, cancellationToken);
        }
    }
}