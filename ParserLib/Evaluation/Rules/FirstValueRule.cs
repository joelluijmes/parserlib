using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    /// <summary>
    ///     Returns the first value in the matched tree. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <seealso cref="ParserLib.Evaluation.Rules.ValueRule{T}" />
    public sealed class FirstValueRule<T> : ValueRule<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FirstValueRule{T}" /> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public FirstValueRule(Rule rule) : base(rule)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FirstValueRule{T}" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rule">The rule.</param>
        public FirstValueRule(string name, Rule rule) : base(name, rule)
        {
        }

        /// <summary>
        ///     Creates the value node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="matchedRule">The matched rule.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new LazyValueNode<T>(name, input, begin, GetValue, matchedRule);

        private static T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            var valueLeafs = valueNode.ChildLeafs.SelectMany(leaf => leaf.WhereLeafs(NodeExtensions.IsValueNode<T>)).Cast<ValueNode<T>>();
            return valueLeafs.First().Value;
        }
    }
}