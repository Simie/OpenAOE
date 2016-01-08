// The MIT License (MIT)
//
// Copyright (c) 2013 Jacob Dufault
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Modified and adapted for use with OpenAOE by Simon Moles
// ReSharper disable StaticMemberInGenericType

using System;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Provides a convenient and efficient way to access a type of Data.
    /// </summary>
    public struct ComponentAccessor
    {
        /// <summary>
        /// Creates a new ComponentAccessor that accesses the specified Data type.
        /// </summary>
        /// <param name="dataType">The type of Data to retrieve; note that this parameter must be a
        /// subtype of Data</param>
        public ComponentAccessor(Type dataType)
            : this(ComponentAccessorFactory.GetId(dataType)) {}

        /// <summary>
        /// Directly construct a ComponentAccessor with the given id.
        /// </summary>
        /// <param name="id">The id of the DataAccessor</param>
        internal ComponentAccessor(int id)
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
        public Type ComponentType => ComponentAccessorFactory.GetType(this);

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(Object obj)
        {
            return obj is ComponentAccessor && this == (ComponentAccessor)obj;
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
        public static bool operator ==(ComponentAccessor x, ComponentAccessor y)
        {
            return x.Id == y.Id;
        }
        /// <summary>
        /// Indicates whether this instance and a specified object are not equal.
        /// </summary>
        public static bool operator !=(ComponentAccessor x, ComponentAccessor y)
        {
            return !(x == y);
        }
    }

    /// <summary>
    /// Map a component interface to its respective accessor, at compile time.
    /// </summary>
    /// <typeparam name="T">The type of Data to map.</typeparam>
    public static class ComponentMap<T> where T : IComponent
    {
        /// <summary>
        /// Gets the accessor for the specified data type.
        /// </summary>
        public static ComponentAccessor Accessor { get; } = new ComponentAccessor(typeof(T));
    }

    /// <summary>
    /// Map a writeable component interface to its respective accessor, at compile time.
    /// </summary>
    /// <typeparam name="T">The type of Data to map.</typeparam>
    public static class WriteableComponentMap<T> where T : IWriteableComponent
    {
        /// <summary>
        /// Gets the accessor for the specified data type.
        /// </summary>
        public static ComponentAccessor Accessor { get; } = new ComponentAccessor(typeof(T));
    }
}
