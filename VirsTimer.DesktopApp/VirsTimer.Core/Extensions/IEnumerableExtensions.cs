using System.Collections.Generic;
using System.Linq;

namespace VirsTimer.Core.Extensions
{
    /// <summary>
    /// Provides extension for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Tells if enumeration is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
