using System;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Restricts access to a code path under certain conditions.
    /// </summary>
    public interface IAccessGate
    {
        /// <summary>
        /// Try and enter the gate.
        /// Do not use this for deterministic logic, it is meant for internal systems to provide their
        /// own error reporting.
        /// </summary>
        /// <returns>True if allowed to enter, false otherwise.</returns>
        bool TryEnter();

        /// <summary>
        /// Enter the gate.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the gate cannot be entered.</exception>
        void Enter();
    }

    internal class AccessGate : IAccessGate
    {
        public bool IsLocked { get; set; }

        public bool TryEnter()
        {
            return !IsLocked;
        }

        public void Enter()
        {
            if (IsLocked)
                throw new InvalidOperationException("Gate is locked.");
        }
    }
}
