namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Matches one or more of the specified rule. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="ParserLib.Parsing.Rules.Rule" />
	public sealed class OneOrMoreRule : Rule
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="OneOrMoreRule"/> class.
		/// </summary>
		/// <param name="rule">The rule.</param>
		public OneOrMoreRule(Rule rule) : base(rule)
        {
        }

		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public override string Definition => $"({FirstChild.Definition})+";

		/// <summary>
		/// Specific rule implementation of the match. Which matches one or more of the specified rule.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal override bool MatchImpl(ParserState state)
        {
            if (!FirstChild.MatchImpl(state))
                return false;

            while (FirstChild.MatchImpl(state))
            {
            }

            return true;
        }
    }
}