using System;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    /// <summary>
    ///     Lazy/derefer converting the matched string to a value. (note that it will also cache the converted value.)
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Evaluation.Nodes.ValueNode{T}" />
    public class LazyValueNode<T> : ValueNode<T>
    {
        private readonly Func<ValueNode<T>, T> _valueFunc;
        private T _value;
        private bool _valueSet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LazyValueNode{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="matchedRule">The matched rule.</param>
        public LazyValueNode(string name, string input, int begin, Func<ValueNode<T>, T> valueFunc, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
            _valueFunc = valueFunc;
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public override T Value
        {
            get
            {
                if (_valueSet)
                    return _value;

                _valueSet = true;
                return _value = _valueFunc(this);
            }
        }
    }
}