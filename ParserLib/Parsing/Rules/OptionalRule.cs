namespace ParserLib.Parsing.Rules
{
    public sealed class OptionalRule : Rule
    {
        public OptionalRule(Rule rule)
            : base(rule)
        {
        }

        public override string Definition => $"{FirstChild}?";

        protected internal override bool MatchImpl(ParserState state)
        {
            FirstChild.MatchImpl(state);
            return true;
        }
    }
}