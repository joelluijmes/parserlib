using System;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public abstract partial class Grammar
    {
        public static readonly Rule Whitespace = Regex("\\s+");
        public static readonly Rule Word = Regex("\\w+");
        public static readonly Rule Digit = Char(char.IsDigit);
        public static readonly Rule Digits = OneOrMore(Digit);
        public static readonly Rule Letter = Char(char.IsLetter);
        public static readonly Rule Letters = OneOrMore(Letter);
        public static readonly Rule Label = (Letter | MatchChar('_')) + ZeroOrMore(Digit | Letter | MatchChar('_'));
        public static readonly Rule PlusOrMinus = MatchChar('+') | MatchChar('-');
        public static readonly Rule E = (MatchChar('e') | MatchChar('E')) + Optional(PlusOrMinus);
        public static readonly Rule Exponential = E + Digits;
        public static readonly Rule Integer = Optional(PlusOrMinus) + Digits + Not(MatchChar('.')) + Optional(Exponential);
        public static readonly Rule Float = Optional(PlusOrMinus) + Digits + MatchChar('.') + Digits + Optional(Exponential);
        public static readonly Rule Hex = Digit | Regex("[a-fA-F]");
        public static readonly Rule HexNumber = MatchString("0x", true) + OneOrMore(Hex);
    }
}