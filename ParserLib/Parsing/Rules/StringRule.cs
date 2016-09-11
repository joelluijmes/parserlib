using System;
using System.Globalization;

namespace ParserLib.Parsing.Rules
{
    public sealed class StringRule : Rule
    {
        public StringRule(string pattern, bool ignoreCase = false)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
            IgnoreCase = ignoreCase;
        }

        public string Pattern { get; }
        public bool IgnoreCase { get; set; }

        public override string Definition => $"\"{Pattern}\"";

        protected internal override bool MatchImpl(ParserState state)
        {
            if (!state.Input.Substring(state.Position).StartsWith(Pattern, IgnoreCase, CultureInfo.InvariantCulture))
                return false;

            state.Position += Pattern.Length;
            return true;
        }
    }
}