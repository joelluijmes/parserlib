namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Matches if zero or more are matched. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="ParserLib.Parsing.Rules.Rule" />
	public sealed class ZeroOrMoreRule : Rule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ZeroOrMoreRule"/> class.
		/// </summary>
		/// <param name="rule">The rule.</param>
		public ZeroOrMoreRule(Rule rule) : base(rule)
		{
		}

		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public override string Definition => $"({FirstChild})*";

		/// <summary>
		/// Specific rule implementation of the match. Which is if one or more children matches.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal override bool MatchImpl(ParserState state)
		{
			while (FirstChild.MatchImpl(state))
			{
			}

			return true;
		}
	}
}