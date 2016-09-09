namespace ParserLib.Parsing.Rules
{
    public sealed class EndRule : Rule
    {
        public override string Definition => "$";
        protected internal override bool MatchImpl(ParserState state) => state.Position == state.Input.Length;
    }
}