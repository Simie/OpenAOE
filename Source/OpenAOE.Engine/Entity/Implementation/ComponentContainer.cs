using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity.Implementation
{
    /// <summary>
    /// Contains a pair of components of the same type so that one can be "written" to and one is
    /// the "current" state.
    /// </summary>
    internal sealed class ComponentContainer
    {
        public IComponent Current { get; }

        public IComponent Next { get; }

        /// <summary>
        /// Create a new instance of the component container with the provided component as the "Current".
        /// "Next" will be created automatically.
        /// </summary>
        /// <param name="component"></param>
        public ComponentContainer(IComponent component)
        {
            Current = component;
            Next = Current.Clone();
        }

        /// <summary>
        /// Apply any changes to "Next" to "Current"
        /// </summary>
        public void CommitChanges()
        {
            Next.CopyTo(Current);
        }
    }
}
