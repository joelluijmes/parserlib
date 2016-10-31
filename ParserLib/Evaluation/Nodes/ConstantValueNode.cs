using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
	/// <summary>
	/// Constant value for matched string.
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
	/// <seealso cref="ParserLib.Evaluation.Nodes.ValueNode{T}" />
	public class ConstantValueNode<T> : ValueNode<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantValueNode{T}"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="input">The input.</param>
		/// <param name="begin">The begin.</param>
		/// <param name="value">The value.</param>
		/// <param name="matchedRule">The matched rule.</param>
		public ConstantValueNode(string name, string input, int begin, T value, Rule matchedRule) : base(name, input, begin, matchedRule)
		{
			Value = value;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public override T Value { get; }
	}
}