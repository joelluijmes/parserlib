using System.Linq;

namespace ParserLib
{
    internal static class Util
    {
        public static T[] MergeArray<T>(T first, T second, params T[] others) =>
            new[] {first, second}.Concat(others).ToArray();
    }
}