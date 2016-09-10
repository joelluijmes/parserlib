using System.Text.RegularExpressions;

namespace ParserLib.Parsing.Rules
{
    public sealed class RegexRule : Rule
    {
        private readonly Regex _regex;

        public RegexRule(string pattern)
            : this(new Regex(pattern))
        {
        }

        public RegexRule(Regex regex)
        {
            _regex = regex;
        }

        public override string Definition => $"regex({_regex})";

        protected internal override bool MatchImpl(ParserState state)
        {
            var match = _regex.Match(state.Input, state.Position);
            if (!match.Success || (match.Index != state.Position))
                return false;

            state.Position += match.Length;
            return true;
        }
    }
}