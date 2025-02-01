using Npgsql.Internal;

using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Accessors
{
    internal static class _PgConverter
    {
        /// <summary>
        /// Calculates the size
        /// of a value in bytes.
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetSizeAsObject")]
        public extern static Size GetSize(PgConverter converter, SizeContext context, object value, ref object writeState);

        /// <summary>
        /// Writes a value to the
        /// <paramref name="writer"/>'s underlying stream and advances 
        /// the position in that stream by the number of bytes written.
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "WriteAsObject")]
        public extern static ValueTask WriteValue(PgConverter converter, bool async, PgWriter writer, object value, CancellationToken cancellationToken);
    }
}