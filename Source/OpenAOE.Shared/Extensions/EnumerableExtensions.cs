using System;
using System.Collections.Generic;

namespace OpenAOE.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Topological sort
        /// </summary>
        /// <remarks>http://stackoverflow.com/a/11027096/147003</remarks>
        public static IEnumerable<T> TSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies);

                sorted.Add(item);
            }
            else if (!sorted.Contains(item))
            {
                throw new Exception("Invalid dependency cycle!");
            }
        }
    }
}
