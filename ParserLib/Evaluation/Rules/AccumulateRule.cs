using System;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class AccumulateRule<T> : ValueRule<T>
    {
        public AccumulateRule(Func<T, T, T> accumulator, Rule rule) : base(rule)
        {
            Accumulator = accumulator;
        }

        public AccumulateRule(string name, Func<T, T, T> accumulator, Rule rule) : base(name, rule)
        {
            Accumulator = accumulator;
        }

        public Func<T, T, T> Accumulator { get; }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new LazyValueNode<T>(name, input, begin, GetValue, matchedRule);

        private T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            // we can't use OfType because we want also the nested leafs
            var valueLeafs = valueNode.ChildLeafs.SelectMany(leaf => leaf.WhereLeafs(NodeExtensions.IsValueNode<T>)).Cast<ValueNode<T>>();

            var current = default(T);
            var first = true;

            foreach (var leaf in valueLeafs)
            {
                current = first
                    ? leaf.Value // set current to the first value
                    : Accumulator(current, leaf.Value); // accumulate the other values

                first = false;
            }

            if (first) // no iteration happend
                throw new EvaluatorException("The AccumulateRule did not yield any results");

            return current;
        }
    }
}