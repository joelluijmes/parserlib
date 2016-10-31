using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
	/// <summary>
	/// Constant value for matched rule. This class cannot be inherited.
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
	/// <seealso cref="ParserLib.Evaluation.Rules.ValueRule{T}" />
	public sealed class ConstantValueRule<T> : ValueRule<T>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantValueRule{T}"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="rule">The rule.</param>
		public ConstantValueRule(T value, Rule rule) : base(rule)
        {
            Value = value;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantValueRule{T}"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="rule">The rule.</param>
		public ConstantValueRule(string name, T value, Rule rule) : base(name, rule)
        {
            Value = value;
        }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public T Value { get; }

		/// <summary>
		/// Creates the value node.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="input">The input.</param>
		/// <param name="begin">The begin.</param>
		/// <param name="matchedRule">The matched rule.</param>
		/// <returns>ValueNode&lt;T&gt;.</returns>
		protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new ConstantValueNode<T>(name, input, begin, Value, matchedRule);
    }
}