using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserLib
{
    internal static class Util
    {
        private static readonly IDictionary<object, bool> _recursiveProcessed = new Dictionary<object, bool>();

        public static T[] MergeArray<T>(T first, params T[] others) =>
            new[] { first }.Concat(others).ToArray();

        public static T[] MergeArray<T>(T first, T second, params T[] others) =>
            new[] { first, second }.Concat(others).ToArray();

        public static string PrettyFormat<T>(T obj, Func<T, IEnumerable<T>> childNodesFunc) where T : class
        {
            lock (_recursiveProcessed)
            {
                _recursiveProcessed.Clear();
                var resultBuilder = new StringBuilder();
                PrettyFormatImpl(null, obj, childNodesFunc, resultBuilder, "", false);

                return resultBuilder.ToString();
            }
        }

        private static void PrettyFormatImpl<T>(T parent, T current, Func<T, IEnumerable<T>> childNodesFunc, StringBuilder resultBuilder, string ident, bool last) where T : class
        {
            if (IsRecursiveElement(parent, current, childNodesFunc))
            {
                bool processed;
                if (_recursiveProcessed.TryGetValue(current, out processed) && processed)
                    return;

                _recursiveProcessed[current] = true;
            }

            resultBuilder.Append(ident);
            if (last)
            {
                resultBuilder.Append("\\- ");
                ident += "   ";
            }
            else
            {
                resultBuilder.Append("|- ");
                ident += "|  ";
            }

            resultBuilder.AppendLine(current.ToString());

            var count = childNodesFunc(current).Count();
            for (var i = 0; i < count; ++i)
            {
                var nextNode = childNodesFunc(current).ElementAt(i);
                PrettyFormatImpl(current, nextNode, childNodesFunc, resultBuilder, ident, (i == count - 1) && childNodesFunc(nextNode).Any());
            }
        }

        private static bool IsRecursiveElement<T>(T parent, T current, Func<T, IEnumerable<T>> childNodesFunc) where T : class
        {
            if (parent == null)
                return false;

            var childs = childNodesFunc(parent);
            return childs.Any(child => (child == current) || IsRecursiveElement(current, child, childNodesFunc));
        }
    }
}