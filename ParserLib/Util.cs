using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParserLib.Parsing.Rules;

namespace ParserLib
{
    internal static class Util
    {
        public static T[] MergeArray<T>(T first, params T[] others) =>
            new[] {first}.Concat(others).ToArray();

        public static T[] MergeArray<T>(T first, T second, params T[] others) =>
            new[] {first, second}.Concat(others).ToArray();

        public static IEnumerable<T> GetStaticFieldsOfType<T>(Type type) => type.GetFields(BindingFlags.Static | BindingFlags.Public).Where(t => t.FieldType == typeof(Rule)).Select(t => (T)t.GetValue(null));
    }
}