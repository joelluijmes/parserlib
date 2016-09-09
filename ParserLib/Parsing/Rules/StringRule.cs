namespace ParserLib.Parsing.Rules
{
    public sealed class StringRule : Rule
    {
        public StringRule(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; }

        public override string Definition => $"\"{Pattern}\"";

        protected internal override bool MatchImpl(ParserState state)
        {
            if (!state.Input.Substring(state.Position).StartsWith(Pattern))
                return false;

            state.Position += Pattern.Length;
            return true;
        }
    }
}