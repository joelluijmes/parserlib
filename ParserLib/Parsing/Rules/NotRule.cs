namespace ParserLib.Parsing.Rules
{
    public sealed class NotRule : Rule
    {
        public NotRule(Rule rule) : base(rule)
        {
        }

        public override string Definition => $"Not({FirstChild.Definition})";

        protected internal override bool MatchImpl(ParserState state)
        {
            var oldState = state.Clone();
            if (!FirstChild.MatchImpl(state))
                return true;

            state = oldState;
            return false;
        }
    }
}