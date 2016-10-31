namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Makes the given rule optional. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="ParserLib.Parsing.Rules.Rule" />
	public sealed class OptionalRule : Rule
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="OptionalRule"/> class.
		/// </summary>
		/// <param name="rule">The rule.</param>
		public OptionalRule(Rule rule) : base(rule)
        {
        }

		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public override string Definition => $"({FirstChild})?";

		/// <summary>
		/// Specific rule implementation of the match. Which always return true.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal override bool MatchImpl(ParserState state)
        {
            FirstChild.MatchImpl(state);
            return true;
        }
    }
}