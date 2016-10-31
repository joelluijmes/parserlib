using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Exceptions;

namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Rule is a base class which is used to match a string input with a specific rule.
	/// </summary>
	public abstract class Rule
	{
		private Rule _not;
		private Rule _oneOrMore;
		private Rule _optional;
		private Rule _zeroOrMore;

		/// <summary>
		/// Initializes a new instance of the <see cref="Rule"/> class.
		/// </summary>
		protected Rule()
		{
			Children = new List<Rule>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Rule"/> class.
		/// </summary>
		/// <param name="children">The children.</param>
		/// <exception cref="System.ArgumentNullException">children</exception>
		protected Rule(IEnumerable<Rule> children)
		{
			if (children == null)
				throw new ArgumentNullException(nameof(children));

			Children = new List<Rule>(children);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Rule"/> class.
		/// </summary>
		/// <param name="firstChild">The first child.</param>
		/// <exception cref="System.ArgumentNullException">firstChild</exception>
		protected Rule(Rule firstChild)
		{
			if (firstChild == null)
				throw new ArgumentNullException(nameof(firstChild));

			Children = new List<Rule> {firstChild};
		}

		/// <summary>
		/// Gets the first child.
		/// </summary>
		/// <value>The first child.</value>
		protected Rule FirstChild => Children.First();
		/// <summary>
		/// Gets the children rules.
		/// </summary>
		/// <value>The children.</value>
		protected List<Rule> Children { get; }
		/// <summary>
		/// Gets or sets the name of the rule.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }
		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public abstract string Definition { get; }

		/// <summary>
		/// Return optional version of this rule.
		/// </summary>
		/// <value>The optional.</value>
		public Rule Optional => _optional ?? (_optional = Grammar.Optional(this));
		/// <summary>
		/// Return not version of this rule.
		/// </summary>
		/// <value>The not.</value>
		public Rule Not => _not ?? (_not = Grammar.Not(this));
		/// <summary>
		/// Return one or more version of this rule.
		/// </summary>
		/// <value>The one or more.</value>
		public Rule OneOrMore => _oneOrMore ?? (_oneOrMore = Grammar.OneOrMore(this));
		/// <summary>
		/// Return zero or more version of this rule.
		/// </summary>
		/// <value>The zero or more.</value>
		public Rule ZeroOrMore => _zeroOrMore ?? (_zeroOrMore = Grammar.ZeroOrMore(this));

		/// <summary>
		/// Build a sequence rule with the specified rules.
		/// </summary>
		/// <param name="left">The left hand side.</param>
		/// <param name="right">The right hand side.</param>
		/// <returns>The result of the operator.</returns>
		public static Rule operator +(Rule left, Rule right) => Grammar.Sequence(left, right);
		/// <summary>
		/// Builds a or rule with the specified rules.
		/// </summary>
		/// <param name="left">The left hand side.</param>
		/// <param name="right">The right hand side.</param>
		/// <returns>The result of the operator.</returns>
		public static Rule operator |(Rule left, Rule right) => Grammar.Or(left, right);
		/// <summary>
		/// Specific rule implementation of the match.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal abstract bool MatchImpl(ParserState state);

		/// <summary>
		/// Matches the specified input.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		/// <exception cref="System.ArgumentNullException">input</exception>
		/// <exception cref="System.ArgumentException">Input should have content - input</exception>
		public bool Match(string input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));
			if (input == string.Empty)
				throw new ArgumentException("Input should have content", nameof(input));

			return MatchImpl(new ParserState(input));
		}

		/// <summary>
		/// Parses input in tree of nodes.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>Node.</returns>
		/// <exception cref="System.ArgumentNullException">input</exception>
		/// <exception cref="ParserException"></exception>
		public Node ParseTree(string input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));

			var state = new ParserState(input);
			if (!MatchImpl(state))
				throw new ParserException($"'{this}' Failed to match '{state.Input}'");

			if (state.Nodes.Count == 1)
				return state.Nodes.First();

			return new Node(ToString(), input, this) {ChildLeafs = state.Nodes};
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString() => Name ?? Definition;
		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <returns>IEnumerable&lt;Rule&gt;.</returns>
		public IEnumerable<Rule> GetChildren() => Children.AsReadOnly();

		/// <summary>
		/// Equalses the specified other.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if equal, <c>false</c> otherwise.</returns>
		protected bool Equals(Rule other)
		{
			return Children.SequenceEqual(other.Children) && string.Equals(Name, other.Name) && string.Equals(Definition, other.Definition);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;

			var other = obj as Rule;
			return (other != null) && Equals(other);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return ((Children?.GetHashCode() ?? 0)*397) ^ ((Name?.GetHashCode() ?? 0)*23) ^ (Definition?.GetHashCode() ?? 0);
			}
		}
	}
}