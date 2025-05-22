using Npgsql.Internal;

using System.Runtime.CompilerServices;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal static class ValueExtension
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(GetSizeAsObject))]
        private extern static Size GetSizeAsObject(PgConverter converter, SizeContext context, object value, ref object writeState);

        /// <summary>
        /// Gets the size of the item in bytes.
        /// </summary>
        public static int GetSizeOrDbNullAsObject(this in Value value, object writeState)
        {
            if (value.IsDbNull)
            {
                return -1;
            }

            return value.BufferRequirements.Write.Kind == SizeKind.Exact ? value.BufferRequirements.Write.Value : GetSizeAsObject(value.Converter, default, value.Item, ref writeState).Value;
        }
    }
}