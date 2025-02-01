using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.PostgresTypes;

using Npgsql.Tvp.Internal.Accessors;
using Npgsql.Tvp.Internal.Converters;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataTableTypeInfoResolver<TTable> : IPgTypeInfoResolver where TTable : DataTable
    {
        private readonly
            TypeInfoMappingCollection _mappings = new
            TypeInfoMappingCollection
            ();

        /// <summary>
        /// Resolves a 
        /// composite type representing a 
        /// <typeparamref name="TTable"/> 
        /// prototype.
        /// </summary>
        private static PostgresCompositeType ResolveCompositeType(Type type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            return dataTypeName != default && type == typeof(TTable) && _NpgsqlDatabaseInfo.Get(options).TryGetPostgresTypeByName(dataTypeName, out PostgresType? pgType) && pgType is PostgresArrayType arrayType && arrayType.Element is PostgresCompositeType compositeType ? compositeType : default;
        }

        /// <summary>
        /// Adds a mapping entry to the collection if 
        /// a composite type exists for the specified 
        /// type.
        /// </summary>
        private TypeInfoMappingCollection AddMapping(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            PostgresCompositeType pgType = ResolveCompositeType(type, dataTypeName, options);

            if (pgType != default)
            {
                _mappings.AddType<TTable>(dataTypeName, (options, mapping, requiresDataTypeName) => mapping.CreateInfo(options, new DataTableConverter<TTable>(pgType, options)));
            }

            return _mappings;
        }

        /// <inheritdoc/>
        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            return _mappings.Find(type, dataTypeName, options) ?? AddMapping(type, dataTypeName, options).Find(type, dataTypeName, options);
        }
    }
}