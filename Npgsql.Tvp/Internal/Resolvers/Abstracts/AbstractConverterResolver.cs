using Npgsql.Internal;
using Npgsql.Internal.Postgres;

namespace Npgsql.Tvp.Internal.Resolvers.Abstracts
{
    internal abstract class AbstractConverterResolver<TParameter>() : PgConverterResolver<TParameter>
    {
        /// <summary>
        /// Gets the array data type.
        /// </summary>
        protected abstract PgTypeId GetArrayType(TParameter value);

        /// <summary>
        /// Gets the converter for the parameter.
        /// </summary>
        protected abstract PgConverter GetConverter();

        /// <inheritdoc/>
        public override PgConverterResolution GetDefault(PgTypeId? pgTypeId)
        {
            return new PgConverterResolution(GetConverter(), pgTypeId.Value);
        }

        /// <inheritdoc/>
        public override PgConverterResolution? Get(TParameter value, PgTypeId? expectedPgTypeId)
        {
            return GetDefault(GetArrayType(value));
        }
    }
}