using Npgsql.Internal;
using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Accessors
{
    internal static class _PgConverter
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetSizeAsObject")]
        public extern static Size GetSize(PgConverter converter, SizeContext context, object value, ref object writeState);

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "WriteAsObjectAsync")]
        public extern static ValueTask WriteValueAsync(PgConverter converter, PgWriter writer, object value, CancellationToken cancellationToken);
    }
}