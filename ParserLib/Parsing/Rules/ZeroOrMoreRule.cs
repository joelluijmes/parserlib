namespace ParserLib.Parsing.Rules
{
    public sealed class ZeroOrMoreRule : Rule
    {
        public ZeroOrMoreRule(Rule rule)
            : base(rule)
        {
        }

        public override string Definition => $"{FirstChild}*";

        protected internal override bool MatchImpl(ParserState state)
        {
            while (FirstChild.MatchImpl(state))
            {
            }

            return true;
        }
    }
}