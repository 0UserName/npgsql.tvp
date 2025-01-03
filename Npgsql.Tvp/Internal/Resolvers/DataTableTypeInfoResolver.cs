using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.PostgresTypes;

using Npgsql.Tvp.Internal.Converters;

using System;
using System.Data;

using System.Runtime.CompilerServices;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataTableTypeInfoResolver<TTable> : IPgTypeInfoResolver where TTable : DataTable
    {
        private readonly
            TypeInfoMappingCollection _mappings = new
            TypeInfoMappingCollection
            ();

        /// <summary>
        /// Checks that the passed type is 
        /// <typeparamref name="TTable"/>.
        /// </summary>
        private static bool IsDataTable(Type type)
        {
            return type != default && type.IsAssignableTo(typeof(TTable));
        }

        /// <summary>
        /// Returns <see cref="NpgsqlDatabaseInfo"/> 
        /// instance that provides information about 
        /// PostgreSQL and PostgreSQL-like databases.
        /// </summary>
        /// 
        /// <remarks>
        /// Unsafe accessor is used because 
        /// instances of this class are not 
        /// publicly accessible.
        /// </remarks>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_DatabaseInfo")]
        private extern static NpgsqlDatabaseInfo GetDatabase(PgSerializerOptions options);

        /// <summary>
        /// 
        /// </summary>
        private void TryAddMapping(DataTypeName dataTypeName, PgSerializerOptions options)
        {
            if (GetDatabase(options).TryGetPostgresTypeByName(dataTypeName, out PostgresType? pgType))
            {
                _mappings.AddType<TTable>(dataTypeName, (options, mapping, requiresDataTypeName) => mapping.CreateInfo(options, new DataTableConverter<TTable>((pgType as PostgresArrayType).Element as PostgresCompositeType, options)), MatchRequirement.All);
            }
        }

        /// <inheritdoc/>
        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            if (IsDataTable(type))
            {
                TryAddMapping(dataTypeName.Value, options);
            }

            return _mappings.Find(type, dataTypeName, options);
        }
    }
}