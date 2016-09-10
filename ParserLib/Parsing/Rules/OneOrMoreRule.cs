namespace ParserLib.Parsing.Rules
{
    public sealed class OneOrMoreRule : Rule
    {
        public OneOrMoreRule(Rule rule)
            : base(rule)
        {
        }

        public override string Definition => $"{FirstChild.Definition}+";

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