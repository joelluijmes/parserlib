using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Exceptions;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    /// <summary>
    ///     Accumulates matched child leafs. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Evaluation.Rules.ValueRule{T}" />
    public sealed class AccumulateRule<T> : ValueRule<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccumulateRule{T}" /> class.
        /// </summary>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="rule">The rule.</param>
        public AccumulateRule(Func<T, T, T> accumulator, Rule rule) : base(rule)
        {
            Accumulator = accumulator;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccumulateRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="rule">The rule.</param>
        public AccumulateRule(string name, Func<T, T, T> accumulator, Rule rule) : base(name, rule)
        {
            Accumulator = accumulator;
        }

        /// <summary>
        ///     Gets the accumulator.
        /// </summary>
        /// <value>The accumulator.</value>
        public Func<T, T, T> Accumulator { get; }

        /// <summary>
        ///     Creates the value node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="matchedRule">The matched rule.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new LazyValueNode<T>(name, input, begin, GetValue, matchedRule);

        private T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            // we can't use OfType because we want also the nested leafs
            var set = new List<ValueNode<T>>();
            var valueLeafs = valueNode.Descendents(node =>
            {
                if (set.Any(cached => cached.Descendents(c => c == node).Any()))
                    return false;

                set.Add(node);
                return true;
            });

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