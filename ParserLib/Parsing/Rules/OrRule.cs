using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Returns true if either of the specified rules are matched. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="ParserLib.Parsing.Rules.Rule" />
	public sealed class OrRule : Rule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrRule"/> class.
		/// </summary>
		/// <param name="firstRule">The first rule.</param>
		/// <param name="secondRule">The second rule.</param>
		/// <param name="rules">The rules.</param>
		public OrRule(Rule firstRule, Rule secondRule, params Rule[] rules)
		{
			var allRules = Util.MergeArray(firstRule, secondRule, rules);
			Children.AddRange(allRules.SelectMany(FlattenRules));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrRule"/> class.
		/// </summary>
		/// <param name="rules">The rules.</param>
		public OrRule(IEnumerable<Rule> rules)
		{
			Children.AddRange(rules.SelectMany(FlattenRules));
		}

		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public override string Definition => $"{Children.Skip(1).Aggregate(FirstChild.ToString(), (a, b) => $"{a} | {b}")}";

		/// <summary>
		/// Specific rule implementation of the match. Which returns true if any child matches.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal override bool MatchImpl(ParserState state)
		{
			var oldState = state.Clone();
			foreach (var child in Children)
			{
				if (child.MatchImpl(state))
					return true;

				state.Assign(oldState);
			}

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString() => $"({base.ToString()})";

		private static IEnumerable<Rule> FlattenRules(Rule r) => r is OrRule ? r.GetChildren() : new[] {r};
	}
}