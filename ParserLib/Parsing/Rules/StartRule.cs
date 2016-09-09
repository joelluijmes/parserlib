namespace ParserLib.Parsing.Rules
{
    public sealed class StartRule : Rule
    {
        public override string Definition => "^";
        protected internal override bool MatchImpl(ParserState state) => state.Position == 0;
    }
}