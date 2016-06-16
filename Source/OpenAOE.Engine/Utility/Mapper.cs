using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Map a type to its respective accessor, at compile time.
    /// </summary>
    /// <typeparam name="TBase">The "root" type, usually an interface. All concrete types will inherit from this.</typeparam>
    public static class Mapper<TBase>
    {
        private static class Factory
        {
            /// <summary>
            /// The next integer to use.
            /// </summary>
            private static int _nextId = 0;

            /// <summary>
            /// The mapping from Data types to their integers
            /// </summary>
            private static readonly Dictionary<Type, int> _ids = new Dictionary<Type, int>();

            /// <summary>
            /// Returns the identifier/integer for the given type, constructing if it necessary.
            /// </summary>
            /// <param name="type">The type to get.</param>
            /// <returns>The identifier/integer</returns>
            public static int GetId(Type type)
            {
                if (!type.IsInterface)
                {
                    throw new ArgumentException(
                        $"Type {type} is not an interface. Did you use a component implementation instead of an interface?",
                        nameof(type));
                }

                if (type == typeof(TBase))
                {
                    throw new ArgumentException(
                        $"Cannot create accessor from {nameof(TBase)}",
                        nameof(type));
                }

                if (!type.GetInterfaces().Contains(typeof(TBase)))
                {
                    throw new ArgumentException($"Type {type} does not inherit from {typeof(TBase)}", nameof(type));
                }

                if (_ids.ContainsKey(type) == false)
                {
                    _ids[type] = _nextId++;
                }

                return _ids[type];
            }

            public static Type GetType(Accessor accessor)
            {
                return _ids.Single(p => p.Value == accessor.Id).Key;
            }
        }

        public struct Accessor
        {
            /// <summary>
            /// Creates a new Accessor for a given type.
            /// </summary>
            /// <param name="dataType">The type of Data to retrieve; note that this parameter must be a
            /// subtype of Data</param>
            public Accessor(Type dataType)
                : this(Factory.GetId(dataType)) { }

            /// <summary>
            /// Directly construct a ComponentAccessor with the given id.
            /// </summary>
            /// <param name="id">The id of the DataAccessor</param>
            internal Accessor(int id)
                : this()
            {
                Id = id;
            }

            /// <summary>
            /// Returns the mapped id for this accessor.
            /// </summary>
            public readonly int Id;

            /// <summary>
            /// Returns the Type object representing the interface of the component this
            /// accessor accesses.
            /// </summary>
            public Type Type => Factory.GetType(this);

            #region Equality Checks
            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            public override bool Equals(Object obj)
            {
                return obj is Accessor && this == (Accessor)obj;
            }
            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            public static bool operator ==(Accessor x, Accessor y)
            {
                return x.Id == y.Id;
            }
            /// <summary>
            /// Indicates whether this instance and a specified object are not equal.
            /// </summary>
            public static bool operator !=(Accessor x, Accessor y)
            {
                return !(x == y);
            }
            #endregion
        }

        /// <summary>
        /// Map a component interface to its respective accessor, at compile time.
        /// </summary>
        /// <typeparam name="TConcrete">The type of Data to map.</typeparam>
        public static class Map<TConcrete> where TConcrete : TBase
        {
            /// <summary>
            /// Gets the accessor for the specified type.
            /// </summary>
            public static Accessor Accessor { get; } = new Accessor(typeof(TConcrete));
        }
    }
}