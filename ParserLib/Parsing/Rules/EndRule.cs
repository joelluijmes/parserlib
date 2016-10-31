namespace ParserLib.Parsing.Rules
{
	/// <summary>
	/// Matches if end of input has been reached. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="ParserLib.Parsing.Rules.Rule" />
	public sealed class EndRule : Rule
	{
		/// <summary>
		/// Gets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public override string Definition => "$";
		/// <summary>
		/// Specific rule implementation of the match.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
		protected internal override bool MatchImpl(ParserState state) => state.Position == state.Input.Length;
	}
}