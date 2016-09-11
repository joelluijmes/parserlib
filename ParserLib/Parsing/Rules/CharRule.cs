using System;
using System.CodeDom.Compiler;

namespace ParserLib.Parsing.Rules
{
    public sealed class CharRule : Rule
    {
        public CharRule(Predicate<char> c)
        {
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            CharPredicate = c;
        }

        public Predicate<char> CharPredicate { get; }

        public override string Definition => CodeGenerator.IsValidLanguageIndependentIdentifier(CharPredicate.Method.Name)
            ? CharPredicate.Method.Name
            : Name ??  "f(char)";

        protected internal override bool MatchImpl(ParserState state)
        {
            if (state.Position >= state.Input.Length)
                return false;

            var result = CharPredicate(state.Input[state.Position]);
            if (result)
                ++state.Position;

            return result;
        }
    }
}