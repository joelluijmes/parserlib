using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
	/// <summary>
	/// ValueRule is a base class which is used for converting matched rules into values.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="ParserLib.Parsing.Rules.NodeRule" />
	public abstract class ValueRule<T> : NodeRule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRule{T}"/> class.
		/// </summary>
		/// <param name="rule">The rule.</param>
		protected ValueRule(Rule rule) : base(rule)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRule{T}"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="rule">The rule.</param>
		protected ValueRule(string name, Rule rule) : base(name, rule)
		{
		}

		/// <summary>
		/// Creates the value node.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="input">The input.</param>
		/// <param name="begin">The begin.</param>
		/// <param name="matchedRule">The matched rule.</param>
		/// <returns>ValueNode&lt;T&gt;.</returns>
		protected abstract ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule);

		/// <summary>
		/// Creates the specific node. Can be overriden.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="input">The input.</param>
		/// <param name="begin">The begin.</param>
		/// <param name="matchedRule">The matched rule.</param>
		/// <returns>Node.</returns>
		protected override Node CreateNode(string name, string input, int begin, Rule matchedRule) => CreateValueNode(name, input, begin, matchedRule);
	}
}