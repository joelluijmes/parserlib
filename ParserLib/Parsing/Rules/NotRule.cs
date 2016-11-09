namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches the inverse of the given rule. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class NotRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NotRule" /> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public NotRule(Rule rule) : base(rule)
        {
        }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => $"Not({FirstLeaf})";

        /// <summary>
        ///     Specific rule implementation of the match. Which inverses the given rule.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state)
        {
            var oldState = state.Clone();
            if (!FirstLeaf.MatchImpl(state))
                return true;

            state.Assign(oldState);
            return false;
        }
    }
}