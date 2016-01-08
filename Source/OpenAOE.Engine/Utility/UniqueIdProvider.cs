using System.Threading;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Provides a unique integer generator with a thread-safe API.
    /// </summary>
    internal sealed class UniqueIdProvider
    {
        private int _next;

        /// <summary>
        /// Create a new UniqueIntProvider that starts at <paramref name="start"/>.
        /// </summary>
        /// <param name="start">The starting integer.</param>
        public UniqueIdProvider(uint start = 0)
        {
            unchecked
            {
                _next = (int) start;
            }
        }

        /// <summary>
        /// Return a unique integer by incrementing the internal counter.
        /// </summary>
        /// <returns>A unique integer.</returns>
        public uint Next()
        {
            var n = Interlocked.Increment(ref _next);
            unchecked
            {
                return (uint) n - 1u;
            }
        }
    }
}
