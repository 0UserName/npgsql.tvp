using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;

namespace Npgsql.Tvp.Internal.Resolvers.Abstract
{
    internal abstract class AbstractConverterResolver<TParameter>(PgSerializerOptions options) : PgConverterResolver<TParameter>
    {
        /// <summary>
        /// Gets the name of a data type.
        /// </summary>
        /// 
        /// <remarks>
        /// The type name must match the name of 
        /// a compatible type previously created 
        /// on the server. 
        /// </remarks>
        protected abstract string GetDataTypeName(TParameter value);

        /// <summary>
        /// Gets the converter for a parameter.
        /// </summary>
        protected abstract PgConverter GetConverter(PgSerializerOptions options);

        /// <inheritdoc/>
        public override PgConverterResolution GetDefault(PgTypeId? pgTypeId)
        {
            return new PgConverterResolution(GetConverter(options), pgTypeId.Value);
        }

        /// <inheritdoc/>
        public override PgConverterResolution? Get(TParameter value, PgTypeId? expectedPgTypeId)
        {
            return GetDefault(options.GetArrayTypeId(new DataTypeName(GetDataTypeName(value) ?? throw new InvalidOperationException("Data type name cannot empty"))));
        }
    }
}