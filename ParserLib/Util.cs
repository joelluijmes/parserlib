using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserLib
{
	/// <summary>
	/// Utility functions that don't belong to the type it self.
	/// </summary>
	internal static class Util
	{
		private static readonly IDictionary<object, bool> _recursiveProcessed = new Dictionary<object, bool>();

		/// <summary>
		/// Merges the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first">The first.</param>
		/// <param name="others">The others.</param>
		/// <returns>T[].</returns>
		public static T[] MergeArray<T>(T first, params T[] others) =>
			new[] {first}.Concat(others).ToArray();

		/// <summary>
		/// Merges the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <param name="others">The others.</param>
		/// <returns>T[].</returns>
		public static T[] MergeArray<T>(T first, T second, params T[] others) =>
			new[] {first, second}.Concat(others).ToArray();

		/// <summary>
		/// Determines whether type is derived from other type. Which takes account for generic type types.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if type is derived from other type; otherwise, <c>false</c>.</returns>
		internal static bool IsDerivedFrom(Type type, Type target)
		{
			while (true)
			{
				if (target == null)
					return false;

				if ((type == target) || (target.IsGenericType && (type == target.GetGenericTypeDefinition())))
					return true;

				target = target.BaseType;
			}
		}

		/// <summary>
		/// Makes a nice tree structure of the Enumerable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="childNodesFunc">The child nodes function.</param>
		/// <returns>System.String.</returns>
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