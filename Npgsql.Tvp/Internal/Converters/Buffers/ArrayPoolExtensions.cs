using System;
using System.Buffers;

namespace Npgsql.Tvp.Internal.Converters.Buffers
{
    internal static class ArrayPoolExtensions
    {
        /// <summary>
        /// Retrieves 
        /// a buffer that is at 
        /// least the requested 
        /// length.
        /// </summary>
        public static T[] Rent<T>(int minimumLength)
        {
            return ArrayPool<T>.Shared.Rent(minimumLength);
        }

        /// <summary>
        /// Returns to the pool an array 
        /// that was previously obtained 
        /// via <see cref="Rent"/>.
        /// </summary>
        /// 
        /// <param name="clearArray">
        /// Indicates whether the contents of the 
        /// buffer should be cleared before reuse.
        /// </param>
        public static void Return<T>(this T[] array, bool clearArray = false)
        {
            ArrayPool<T>.Shared.Return(array, clearArray);
        }

        /// <inheritdoc cref="Return{T}(T[], bool)"/>
        /// 
        /// <param name="start">
        /// The starting index of 
        /// the range of elements 
        /// to clear.
        /// </param>
        /// 
        /// <param name="length">
        /// The number of elements to clear.
        /// </param>
        public static void Return<T>(this T[] array, int start, int length)
        {
            array.AsSpan(start, length).Clear();

            array.Return();
        }

        /// <summary>
        /// Resizes a 
        /// one-dimensional array 
        /// to twice its original 
        /// length.
        /// </summary>
        /// 
        /// <param name="clearArray">
        /// Indicates whether the contents of the 
        /// buffer should be cleared before reuse.
        /// </param>
        public static T[] Resize<T>(this T[] array, bool clearArray = false)
        {
            T[] newArray = Rent<T>(array.Length * 2);

            array.AsSpan().CopyTo(newArray);

            array.Return(clearArray);

            return newArray;
        }
    }
}