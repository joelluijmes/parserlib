using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Provides base class for collection of rules
    /// </summary>
    public abstract partial class Grammar
    {
        /// <summary>
        ///     Matches whitespace
        /// </summary>
        public static readonly Rule Whitespace = Regex("\\s+");

        /// <summary>
        ///     Matches single word
        /// </summary>
        public static readonly Rule Word = Regex("\\w+");

        /// <summary>
        ///     Matches single digit
        /// </summary>
        public static readonly Rule Digit = Char(char.IsDigit);

        /// <summary>
        ///     Matches digits
        /// </summary>
        public static readonly Rule Digits = OneOrMore(Digit);

        /// <summary>
        ///     Matches single letter
        /// </summary>
        public static readonly Rule Letter = Char(char.IsLetter);

        /// <summary>
        ///     Matches letters
        /// </summary>
        public static readonly Rule Letters = OneOrMore(Letter);

        /// <summary>
        ///     Matches a label (can only start with a letter or '_' followed by any letter, digit or '_')
        /// </summary>
        public static readonly Rule Label = (Letter | MatchChar('_')) + ZeroOrMore(Digit | Letter | MatchChar('_'));

        /// <summary>
        ///     Matches plus or minus sign
        /// </summary>
        public static readonly Rule PlusOrMinus = MatchChar('+') | MatchChar('-');

        /// <summary>
        ///     Matches e
        /// </summary>
        public static readonly Rule E = (MatchChar('e') | MatchChar('E')) + Optional(PlusOrMinus);

        /// <summary>
        ///     Matches exponential
        /// </summary>
        public static readonly Rule Exponential = E + Digits;

        /// <summary>
        ///     Matches integer
        /// </summary>
        public static readonly Rule Integer = Optional(PlusOrMinus) + Digits + Not(MatchChar('.')) + Optional(Exponential);

        /// <summary>
        ///     Matches float
        /// </summary>
        public static readonly Rule Float = Optional(PlusOrMinus) + Digits + MatchChar('.') + Digits + Optional(Exponential);

        /// <summary>
        ///     Matches single hexadecimal character
        /// </summary>
        public static readonly Rule Hex = Digit | Regex("[a-fA-F]");

        /// <summary>
        ///     Matches hexadecimal number (prepended by 0x or followed 'h')
        /// </summary>
        public static readonly Rule HexNumber = (MatchString("0x", true) + OneOrMore(Hex)) | (OneOrMore(Hex) + MatchChar('h'));
    }
}