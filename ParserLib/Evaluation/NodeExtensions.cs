using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;

namespace ParserLib.Evaluation
{
    public static class NodeExtensions
    {
        public static T Process<T>(this Node root, Func<T, T, T> accumulator) =>
            ProcessImpl(root, accumulator, default(T));

        private static T ProcessImpl<T>(this Node node, Func<T, T, T> accumulator, T current)
        {
            T value;
            if (node.TryGetValue(out value))
                return accumulator(current, value);

            foreach (var leaf in node.Leafs)
                current = accumulator(current, ProcessImpl(leaf, accumulator, current));

            return current;
        }

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

        public static T FirstValue<T>(this Node root)
        {
            var valueNode = root.FirstValueNode<T>();
            return valueNode.Value;
        }

        public static T FirstValueOrDefault<T>(this Node root)
        {
            var valueNode = root.FirstValueNodeOrDefault<T>();
            return valueNode == null
                ? default(T)
                : valueNode.Value;
        }

        public static T FirstValueByName<T>(this Node root, string name)
        {
            var valueNode = (ValueNode<T>)root.WhereLeafs(node => IsValueNode<T>(node) && (node.Name == name)).FirstOrDefault();
            if (valueNode == null)
                throw new EvaluatorException($"ValueNode with name '{name}' not found");

            return valueNode.Value;
        }

        public static T FirstValueByNameOrDefault<T>(this Node root, string name)
        {
            var valueNode = (ValueNode<T>) root.WhereLeafs(node => IsValueNode<T>(node) && (node.Name == name)).FirstOrDefault();
            return valueNode == null ? default(T) : valueNode.Value;
        }

        public static Node FirstNodeByName(this Node root, string name)
        {
            var node = root.FirstNodeByNameOrDefault(name);
            if (node == null)
                throw new EvaluatorException($"Node with name '{name}' not found");

            return node;
        }

        public static Node FirstNodeByNameOrDefault(this Node root, string name) =>
            root.WhereLeafs(node => node.Name == name).FirstOrDefault();

        public static ValueNode<T> FirstValueNode<T>(this Node root) =>
            (ValueNode<T>) root.WhereLeafs(IsValueNode<T>).First();

        public static ValueNode<T> FirstValueNodeOrDefault<T>(this Node root) =>
            (ValueNode<T>) root.WhereLeafs(IsValueNode<T>).FirstOrDefault();

        public static bool ContainsValueNode(this Node root) =>
            root.WhereLeafs(IsValueNode).FirstOrDefault() != null;

        public static bool ContainsValueNode<T>(this Node root) =>
            root.FirstValueNodeOrDefault<T>() != null;

        public static bool IsValueNode(this Node node) =>
            IsDerivedFrom(typeof(ValueNode<>), node.GetType());

        public static bool IsValueNode<T>(this Node node) =>
            IsDerivedFrom(typeof(ValueNode<T>), node.GetType());

        public static IEnumerable<Node> WhereLeafs(this Node branch, Predicate<Node> predicate)
        {
            if (predicate(branch))
                yield return branch;

            foreach (var leaf in branch.Leafs)
                foreach (var subLeaf in leaf.WhereLeafs(predicate))
                    yield return subLeaf;
        }

        private static bool IsDerivedFrom(Type type, Type target)
        {
            if (target == null)
                return false;

            if ((type == target) || (target.IsGenericType && (type == target.GetGenericTypeDefinition())))
                return true;

            return IsDerivedFrom(type, target.BaseType);
        }
    }
}