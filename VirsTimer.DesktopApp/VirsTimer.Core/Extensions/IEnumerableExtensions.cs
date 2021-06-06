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

        /// <summary>
        /// Returns collection of stepped subcollections that contains <paramref name="step"/> elements each.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> StepCollection<T>(this ICollection<T> source, int step)
        {
            for (var i = 0; i < source.Count - step; i++)
                yield return source.Skip(i).Take(step);
        }
    }
}
