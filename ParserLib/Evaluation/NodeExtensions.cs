using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Exceptions;
using ParserLib.Parsing;

namespace ParserLib.Evaluation
{
	/// <summary>
	/// Useful extensions on node which gets the values in tree of nodes.
	/// </summary>
	public static class NodeExtensions
	{
		/// <summary>
		/// Processes the specified input using an accumulator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <param name="accumulator">The accumulator.</param>
		/// <returns>T.</returns>
		public static T Process<T>(this Node root, Func<T, T, T> accumulator) =>
			ProcessImpl(root, accumulator, default(T));

		private static T ProcessImpl<T>(this Node node, Func<T, T, T> accumulator, T current)
		{
			T value;
			if (node.TryGetValue(out value))
				return accumulator(current, value);

			foreach (var leaf in node.ChildLeafs)
				current = accumulator(current, ProcessImpl(leaf, accumulator, current));

			return current;
		}

		/// <summary>
		/// Tries the get value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node">The node.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool TryGetValue<T>(this Node node, out T value)
		{
			if (node.IsValueNode<T>())
			{
				value = ((ValueNode<T>) node).Value;
				return true;
			}

			value = default(T);
			return false;
		}

		/// <summary>
		/// Finds the first value in the tree.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <returns>T.</returns>
		/// <exception cref="ParserLib.Exceptions.EvaluatorException">ValueNode not found</exception>
		public static T FirstValue<T>(this Node root)
		{
			var valueNode = root.FirstValueNodeOrDefault<T>();
			if (valueNode == null)
				throw new EvaluatorException("ValueNode not found");

			return valueNode.Value;
		}

		/// <summary>
		/// Finds the first value in the tree or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <returns>T.</returns>
		public static T FirstValueOrDefault<T>(this Node root)
		{
			var valueNode = root.FirstValueNodeOrDefault<T>();
			return valueNode == null
				? default(T)
				: valueNode.Value;
		}

		/// <summary>
		/// Finds the first value in tree by name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <returns>T.</returns>
		/// <exception cref="ParserLib.Exceptions.EvaluatorException"></exception>
		public static T FirstValueByName<T>(this Node root, string name)
		{
			var valueNode = (ValueNode<T>) root.WhereLeafs(node => IsValueNode<T>(node) && (node.Name == name)).FirstOrDefault();
			if (valueNode == null)
				throw new EvaluatorException($"ValueNode with name '{name}' not found");

			return valueNode.Value;
		}

		/// <summary>
		/// Finds the first value in the tree by name or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <returns>T.</returns>
		public static T FirstValueByNameOrDefault<T>(this Node root, string name)
		{
			var valueNode = (ValueNode<T>) root.WhereLeafs(node => IsValueNode<T>(node) && (node.Name == name)).FirstOrDefault();
			return valueNode == null ? default(T) : valueNode.Value;
		}

		/// <summary>
		/// Finds the first node in the tree by name.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <returns>Node.</returns>
		/// <exception cref="ParserLib.Exceptions.EvaluatorException"></exception>
		public static Node FirstNodeByName(this Node root, string name)
		{
			var node = root.FirstNodeByNameOrDefault(name);
			if (node == null)
				throw new EvaluatorException($"Node with name '{name}' not found");

			return node;
		}

		/// <summary>
		/// Finds the first node in the tree by name or default.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <returns>Node.</returns>
		public static Node FirstNodeByNameOrDefault(this Node root, string name) =>
			root.WhereLeafs(node => node.Name == name).FirstOrDefault();

		/// <summary>
		/// Finds the first value in the tree node.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <returns>ValueNode&lt;T&gt;.</returns>
		public static ValueNode<T> FirstValueNode<T>(this Node root) =>
			(ValueNode<T>) root.WhereLeafs(IsValueNode<T>).First();

		/// <summary>
		/// Finds the first value in the tree node or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <returns>ValueNode&lt;T&gt;.</returns>
		public static ValueNode<T> FirstValueNodeOrDefault<T>(this Node root) =>
			(ValueNode<T>) root.WhereLeafs(IsValueNode<T>).FirstOrDefault();

		/// <summary>
		/// Determines whether contains value node in tree.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <returns><c>true</c> if contains value node in tree; otherwise, <c>false</c>.</returns>
		public static bool ContainsValueNode(this Node root) =>
			root.WhereLeafs(IsValueNode).FirstOrDefault() != null;

		/// <summary>
		/// Determines whether contains value node in tree.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <returns><c>true</c> if contains value node in tree; otherwise, <c>false</c>.</returns>
		public static bool ContainsValueNode<T>(this Node root) =>
			root.FirstValueNodeOrDefault<T>() != null;

		/// <summary>
		/// Determines whether Node is ValueNode.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns><c>true</c> if Node is ValueNode; otherwise, <c>false</c>.</returns>
		public static bool IsValueNode(this Node node) =>
			Util.IsDerivedFrom(typeof(ValueNode<>), node.GetType());

		/// <summary>
		/// Determines whether Node is ValueNode.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node">The node.</param>
		/// <returns><c>true</c> if Node is ValueNode; otherwise, <c>false</c>.</returns>
		public static bool IsValueNode<T>(this Node node) =>
			Util.IsDerivedFrom(typeof(ValueNode<T>), node.GetType());

		/// <summary>
		/// Wheres the leafs matches a predicate.
		/// </summary>
		/// <param name="branch">The branch.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>IEnumerable&lt;Node&gt;.</returns>
		public static IEnumerable<Node> WhereLeafs(this Node branch, Predicate<Node> predicate)
		{
			if (predicate(branch))
				yield return branch;

			foreach (var leaf in branch.ChildLeafs)
				foreach (var subLeaf in leaf.WhereLeafs(predicate))
					yield return subLeaf;
		}
	}
}