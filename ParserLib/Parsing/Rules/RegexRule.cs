using System;
using System.Text.RegularExpressions;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches a specific regex pattern. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class RegexRule : Rule
    {
        private readonly Regex _regex;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegexRule" /> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public RegexRule(string pattern) : this(new Regex(pattern))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegexRule" /> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <exception cref="System.ArgumentNullException">regex</exception>
        public RegexRule(Regex regex)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            _regex = regex;
        }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => $"regex({_regex})";

        /// <summary>
        ///     Specific rule implementation of the match. Which matches the specified regex pattern.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
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