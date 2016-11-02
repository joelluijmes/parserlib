using System;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    /// <summary>
    ///     Convertes matched string to a value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Evaluation.Nodes.ValueNode{T}" />
    public class ConvertToValueNode<T> : ValueNode<T>
    {
        private readonly Func<ValueNode<T>, T> _valueFunc;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConvertToValueNode{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="matchedRule">The matched rule.</param>
        public ConvertToValueNode(string name, string input, int begin, Func<ValueNode<T>, T> valueFunc, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
            _valueFunc = valueFunc;
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public override T Value => _valueFunc(this);
    }
}