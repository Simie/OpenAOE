using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.System;
using OpenAOE.Extensions;

namespace OpenAOE.Engine.Utility
{
    public static class ExecuteOrderSorter
    {
        /// <summary>
        /// Sort a list based on the ExecuteOrderAttribute.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> Sort<T>(IList<T> list)
        {
            // Create a lookup we can use for TSort
            var lookup = new Dictionary<T, List<T>>();
            foreach (var x in list)
            {
                lookup.Add(x, new List<T>());
            }
            
            foreach (var x in list)
            {
                var type = x.GetType();
                var constraints =
                    Attribute.GetCustomAttributes(type, typeof (ExecuteOrderAttribute)).Cast<ExecuteOrderAttribute>();

                foreach (var attr in constraints)
                {
                    var other = list.SingleOrDefault(p => attr.Other.IsInstanceOfType(p));

                    if (other == null)
                        continue;

                    if (attr.Position == ExecuteOrderAttribute.Positions.Before)
                    {
                        // If before, add the current system as a dependency of the other system.
                        lookup[other].Add(x);
                    }
                    else if (attr.Position == ExecuteOrderAttribute.Positions.After)
                    {
                        // If after, add the other system as a dependency of the current system.
                        lookup[x].Add(other);
                    }
                }
            }

            // Sort systems by dependencies
            return list.TSort(system => lookup[system]).ToList();
        }
    }
}