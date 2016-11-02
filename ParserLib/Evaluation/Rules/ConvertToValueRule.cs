using System;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    /// <summary>
    ///     Convertes matched string to value. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Evaluation.Rules.ValueRule{T}" />
    public sealed class ConvertToValueRule<T> : ValueRule<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConvertToValueRule{T}" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="rule">The rule.</param>
        public ConvertToValueRule(Func<Node, T> converter, Rule rule) : base(rule)
        {
            Converter = converter;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConvertToValueRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="rule">The rule.</param>
        public ConvertToValueRule(string name, Func<Node, T> converter, Rule rule) : base(name, rule)
        {
            Converter = converter;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConvertToValueRule{T}" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="rule">The rule.</param>
        public ConvertToValueRule(Func<string, T> converter, Rule rule) : base(rule)
        {
            Converter = node => converter(node.Text);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConvertToValueRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="rule">The rule.</param>
        public ConvertToValueRule(string name, Func<string, T> converter, Rule rule) : base(name, rule)
        {
            Converter = node => converter(node.Text);
        }

        /// <summary>
        ///     Gets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public Func<Node, T> Converter { get; }

        /// <summary>
        ///     Creates the value node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="matchedRule">The matched rule.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new ConvertToValueNode<T>(name, input, begin, Converter, matchedRule);
    }
}