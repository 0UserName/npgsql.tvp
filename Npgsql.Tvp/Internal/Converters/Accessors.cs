using Npgsql.Internal;

using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class Accessors
    {
        /// <summary>
        /// Accessor for:
        /// 
        /// <code>
        /// internal static class PgConverterExtensions
        /// {
        ///     public static Size? GetSizeOrDbNullAsObject(this PgConverter converter, DataFormat format, Size writeRequirement, object? value, ref object? writeState)
        /// }
        /// </code>
        /// </summary>
        /// 
        /// <param name="this">
        /// Always null.
        /// </param>
        /// 
        /// <param name="format">
        /// Always <see cref="DataFormat.Binary"/>.
        /// </param>
        /// 
        /// <param name="writeState">
        /// Always null.
        /// </param>
        [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = nameof(GetSizeOrDbNullAsObject))]
        public extern static Size? GetSizeOrDbNullAsObject([UnsafeAccessorType("Npgsql.Internal.PgConverterExtensions, Npgsql")] object @this, PgConverter converter, DataFormat format, Size writeRequirement, object? value, ref object? writeState);

        /// <summary>
        /// Accessor for:
        /// 
        /// <code>
        /// public abstract class PgConverter
        /// {
        ///     internal ValueTask WriteAsObjectAsync(PgWriter writer, object value, CancellationToken cancellationToken = default)
        /// }
        /// </code>
        /// </summary>
        /// 
        /// <param name="converter">
        /// Instance.
        /// </param>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(WriteAsObjectAsync))]
        public extern static ValueTask WriteAsObjectAsync(PgConverter converter, PgWriter writer, object value, CancellationToken cancellationToken);
    }
}