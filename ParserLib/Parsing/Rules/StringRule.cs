using System;
using System.Globalization;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches a specific string in the input. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class StringRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StringRule" /> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <exception cref="System.ArgumentNullException">pattern</exception>
        public StringRule(string pattern, bool ignoreCase = false)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
            IgnoreCase = ignoreCase;
        }

        /// <summary>
        ///     Gets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
        public string Pattern { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether [ignore case].
        /// </summary>
        /// <value><c>true</c> if [ignore case]; otherwise, <c>false</c>.</value>
        public bool IgnoreCase { get; set; }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => $"\"{Pattern}\"";

        /// <summary>
        ///     Specific rule implementation of the match. Which returns true if the string matches.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state)
        {
            if (!state.Input.Substring(state.Position).StartsWith(Pattern, IgnoreCase, CultureInfo.InvariantCulture))
                return false;

            state.Position += Pattern.Length;
            return true;
        }
    }
}