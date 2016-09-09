using System;

namespace ParserLib.Parsing.Rules
{
    public sealed class CharRule : Rule
    {
        public CharRule(Predicate<char> c)
        {
            CharPredicate = c;
        }

        public Predicate<char> CharPredicate { get; }

        public override string Definition => "f(char)";

        protected internal override bool MatchImpl(ParserState state)
        {
            var result = CharPredicate(state.Input[state.Position]);
            if (result)
                ++state.Position;

            return result;
        }
    }
}