using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.Utility
{
    public interface IComponentFilter
    {
        /// <summary>
        /// Check that <paramref name="target"/> passes this component filter.
        /// </summary>
        /// <param name="target">Object to check against the filter.</param>
        /// <returns>True if <paramref name="target"/> passes the filter.</returns>
        bool Filter(IHasComponents target);
    }
}
