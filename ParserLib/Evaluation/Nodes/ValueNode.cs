using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    /// <summary>
    ///     Base class node rules which should be converted to a value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Parsing.Node" />
    public abstract class ValueNode<T> : Node
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueNode{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The first position to start parsing.</param>
        /// <param name="matchedRule">The matched rule.</param>
        protected ValueNode(string name, string input, int begin, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public abstract T Value { get; }
    }
}