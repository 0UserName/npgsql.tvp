using Npgsql.Internal;

using System;
using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Accessors
{
    [Obsolete("Used until the required methods become public")]
    internal static class _PgConverter
    {
        /// <summary>
        /// 
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(GetSizeAsObject))]
        public extern static Size GetSizeAsObject(PgConverter converter, SizeContext context, object value, ref object writeState);

        /// <summary>
        /// 
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(WriteAsObjectAsync))]
        public extern static ValueTask WriteAsObjectAsync(PgConverter converter, PgWriter writer, object value, CancellationToken cancellationToken);
    }
}