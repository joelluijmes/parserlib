using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;

namespace ParserLib.Evaluation
{
    public static class Evaluator
    {
        public static T Process<T>(Node root, Func<T, T, T> accumulator) =>
            ProcessImpl(root, accumulator, default(T));

        private static T ProcessImpl<T>(Node node, Func<T, T, T> accumulator, T current)
        {
            if (IsValueNode<T>(node))
                return accumulator(current, ((ValueNode<T>) node).Value);

            foreach (var leaf in node.Leafs)
                current = accumulator(current, ProcessImpl(leaf, accumulator, current));

            return current;
        }

        public static Node FirstValueNodeOrDefault(Node root) =>
            FirstLeafOrDefault(root, IsValueNode);

        public static ValueNode<T> FirstValueNodeOrDefault<T>(Node root) =>
            (ValueNode<T>) FirstLeafOrDefault(root, IsValueNode<T>);

        public static bool ContainsValueNode(Node root) =>
            FirstValueNodeOrDefault(root) != null;

        public static bool ContainsValueNode<T>(Node root) =>
            FirstValueNodeOrDefault<T>(root) != null;

        public static T FirstValue<T>(Node root)
        {
            var valueNode = FirstValueNodeOrDefault<T>(root);
            if (valueNode == null)
                throw new EvaluatorException("Value node not found in tree");

            return valueNode.Value;
        }

        public static T FirstValueOrDefault<T>(Node root)
        {
            var valueNode = FirstValueNodeOrDefault<T>(root);
            return valueNode == null
                ? default(T)
                : valueNode.Value;
        }

        public static bool IsValueNode(Node node) =>
            IsDerivedFrom(typeof(ValueNode<>), node.GetType());

        public static bool IsValueNode<T>(Node node) =>
            IsDerivedFrom(typeof(ValueNode<T>), node.GetType());

        public static Node FirstLeafOrDefault(Node branch, Predicate<Node> predicate) =>
            WhereLeafs(branch, predicate).FirstOrDefault();

        public static IEnumerable<Node> WhereLeafs(Node branch, Predicate<Node> predicate)
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