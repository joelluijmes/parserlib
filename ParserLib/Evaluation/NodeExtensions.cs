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
            if (IsValueNode<T>(node))
                return accumulator(current, ((ValueNode<T>) node).Value);

            foreach (var leaf in node.Leafs)
                current = accumulator(current, ProcessImpl(leaf, accumulator, current));

            return current;
        }

        public static Node FirstNodeByName(this Node root, string name) =>
            FirstLeafOrDefault(root, node => node.Name == name);

        public static T ValueByName<T>(this Node root, string name)
        {
            var valueNode = (ValueNode<T>)FirstLeafOrDefault(root, node => IsValueNode<T>(node) && (node.Name == name));
            if (valueNode == null)
                throw new EvaluatorException("Value node not found in tree");

            return valueNode.Value;
        }

        public static Node FirstValueNodeOrDefault(this Node root) =>
            FirstLeafOrDefault(root, IsValueNode);

        public static ValueNode<T> FirstValueNodeOrDefault<T>(this Node root) =>
            (ValueNode<T>) FirstLeafOrDefault(root, IsValueNode<T>);

        public static bool ContainsValueNode(this Node root) =>
            FirstValueNodeOrDefault(root) != null;

        public static bool ContainsValueNode<T>(this Node root) =>
            FirstValueNodeOrDefault<T>(root) != null;

        public static T FirstValue<T>(this Node root)
        {
            var valueNode = FirstValueNodeOrDefault<T>(root);
            if (valueNode == null)
                throw new EvaluatorException("Value node not found in tree");

            return valueNode.Value;
        }

        public static T FirstValueOrDefault<T>(this Node root)
        {
            var valueNode = FirstValueNodeOrDefault<T>(root);
            return valueNode == null
                ? default(T)
                : valueNode.Value;
        }

        public static bool IsValueNode(this Node node) =>
            IsDerivedFrom(typeof(ValueNode<>), node.GetType());

        public static bool IsValueNode<T>(this Node node) =>
            IsDerivedFrom(typeof(ValueNode<T>), node.GetType());

        public static Node FirstLeafOrDefault(this Node branch, Predicate<Node> predicate) =>
            WhereLeafs(branch, predicate).FirstOrDefault();

        public static IEnumerable<Node> WhereLeafs(this Node branch, Predicate<Node> predicate)
        {
            if (predicate(branch))
                yield return branch;

            foreach (var leaf in branch.Leafs)
                foreach (var subLeaf in WhereLeafs(leaf, predicate))
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