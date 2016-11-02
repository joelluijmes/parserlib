namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches if it is at the begin of the string. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class StartRule : Rule
    {
        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => "^";

        /// <summary>
        ///     Specific rule implementation of the match. Which is when state.Position is at 0.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state) => state.Position == 0;
    }
}