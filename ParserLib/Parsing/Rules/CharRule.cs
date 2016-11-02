using System;
using System.CodeDom.Compiler;
using ParserLib.Exceptions;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches specific character using a predicate. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class CharRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CharRule" /> class.
        /// </summary>
        /// <param name="c">Predicate of the character.</param>
        /// <exception cref="System.ArgumentNullException">c</exception>
        public CharRule(Predicate<char> c)
        {
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            CharPredicate = c;
        }

        /// <summary>
        ///     Gets the character predicate.
        /// </summary>
        /// <value>The character predicate.</value>
        public Predicate<char> CharPredicate { get; }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => CodeGenerator.IsValidLanguageIndependentIdentifier(CharPredicate.Method.Name)
            ? CharPredicate.Method.Name
            : Name ?? "f(char)";

        /// <summary>
        ///     Specific rule implementation of the match.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        /// <exception cref="ParserException">
        ///     new InvalidOperationException("Position is longer than input string (end of string
        ///     has been reached)")
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Position is longer than input string (end of string has been
        ///     reached)
        /// </exception>
        protected internal override bool MatchImpl(ParserState state)
        {
            if (state.Position == state.Input.Length)
                return false; // end of input
            if (state.Position > state.Input.Length)
                throw new ParserException(new InvalidOperationException("Position is longer than input string (end of string has been reached)"));

            var result = CharPredicate(state.Input[state.Position]);
            if (result)
                ++state.Position;

            return result;
        }
    }
}