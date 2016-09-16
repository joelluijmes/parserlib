using System;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class EvaluateLeafsRule<T> : ValueRule<T>
    {
        public EvaluateLeafsRule(string name, Func<T, T, T> accumulator, Rule rule) : base(name, rule)
        {
            Accumulator = accumulator;
        }

        public Func<T, T, T> Accumulator { get; }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin) => new LazyValueNode<T>(name, input, begin, GetValue);

        private T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            var valueLeafs = valueNode.Leafs.SelectMany(leaf => NodeExtensions.WhereLeafs(leaf, NodeExtensions.IsValueNode<T>)).Cast<ValueNode<T>>();

            var current = default(T);
            var first = true;

            foreach (var leaf in valueLeafs)
            {
                current = first
                    ? leaf.Value
                    : Accumulator(current, leaf.Value);

                first = false;
            }

            if (first) // no iteration happend
                throw new NotImplementedException();

            return current;
        }
    }
}