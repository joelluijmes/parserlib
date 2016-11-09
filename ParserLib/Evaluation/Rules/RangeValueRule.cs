using System.Collections.Generic;
using ParserLib.Evaluation.Nodes;
using ParserLib.Exceptions;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    /// <summary>
    ///     Returns the first value in the matched tree after validating if the value lies in the expected range. This class
    ///     cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ParserLib.Evaluation.Rules.ValueRule{T}" />
    public sealed class RangeValueRule<T> : ValueRule<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeValueRule{T}" /> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="rule">The rule.</param>
        public RangeValueRule(T minimum, T maximum, Rule rule) : base(rule)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = Comparer<T>.Default;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeValueRule{T}" /> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="rule">The rule.</param>
        public RangeValueRule(T minimum, T maximum, IComparer<T> comparer, Rule rule) : base(rule)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = comparer;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeValueRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="rule">The rule.</param>
        public RangeValueRule(string name, T minimum, T maximum, Rule rule) : base(name, rule)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = Comparer<T>.Default;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeValueRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="rule">The rule.</param>
        public RangeValueRule(string name, T minimum, T maximum, IComparer<T> comparer, Rule rule) : base(name, rule)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = comparer;
        }

        /// <summary>
        ///     Gets the minimum value specified in the constructor.
        /// </summary>
        /// <value>The minimum.</value>
        public T Minimum { get; }

        /// <summary>
        ///     Gets the maximum value specified in the constructor.
        /// </summary>
        /// <value>The maximum.</value>
        public T Maximum { get; }

        /// <summary>
        ///     Gets the comparer specified in the constructor or the default T comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<T> Comparer { get; }

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
            var value = FirstLeaf.FirstValue<T>(valueNode.Text);

            if ((Comparer.Compare(Minimum, value) > 0) || (Comparer.Compare(Maximum, value) < 0))
                throw new EvaluatorException($"The value doesn't meet the expected range. Minimum '{Minimum}'. Maximum '{Maximum}'. Actual '{value}'.");

            return value;
        }
    }
}