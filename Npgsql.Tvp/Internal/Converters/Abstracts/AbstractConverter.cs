using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters.Abstracts
{
    internal abstract class AbstractConverter<TParameter> : PgStreamingConverter<TParameter>
    {
        protected abstract IParameter GetParameter(TParameter value);

        /// <inheritdoc/>
        public override TParameter Read(PgReader reader)
        {
            throw new NotImplementedException($"{ typeof(TParameter).FullName } is not supported");
        }

        /// <inheritdoc/>
        public override ValueTask<TParameter> ReadAsync(PgReader reader, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException($"{ typeof(TParameter).FullName } is not supported");
        }

        /// <inheritdoc/>
        public override Size GetSize(SizeContext context, TParameter value, ref object writeState)
        {
            IParameter parameter = GetParameter(value);

            writeState = parameter;

            return parameter.GetSize();
        }

        /// <inheritdoc/>
        public override void Write(PgWriter writer, TParameter value)
        {
            WriteAsync(writer, value).AsTask().Wait();
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(PgWriter writer, TParameter value, CancellationToken cancellationToken = default)
        {
            return ParameterWriter.WriteAsync(writer, cancellationToken);
        }
    }
}