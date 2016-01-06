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

using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;
using Shouldly;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Maps different types of Data to a sequential set of integers.
    /// </summary>
    internal static class ComponentAccessorFactory
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

            if (type == typeof (IComponent) || type == typeof(IWriteableComponent) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IWriteableComponent<>))
            {
                throw new ArgumentException(
                    $"Cannot create accessor from {typeof (IComponent)} or {typeof(IWriteableComponent)}",
                    nameof(type));
            }

            // If the Type is a writable component, find the read-only interface.
            var writeableComponentType = type.GetInterfaces().SingleOrDefault(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IWriteableComponent<>));
            if (writeableComponentType != null)
            {
                var genericArguments = writeableComponentType.GetGenericArguments();
                type = genericArguments.Single();
            }

            if (!type.GetInterfaces().Contains(typeof (IComponent)))
            {
                throw new ArgumentException($"Type {type} does not inherit from {typeof(IComponent)}", nameof(type));
            }

            if (_ids.ContainsKey(type) == false)
            {
                _ids[type] = _nextId++;
            }

            return _ids[type];
        }

        public static Type GetType(ComponentAccessor accessor)
        {
            return _ids.Single(p => p.Value == accessor.Id).Key;
        }
    }
}