﻿using System;
using System.Linq;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public sealed class SharedGrammar : Grammar
    {
        public static Rule Whitespace = Regex("\\s+");
        public static Rule Word = Regex("\\w+");
        public static Rule Digit = Char(char.IsDigit);
        public static Rule Digits = OneOrMore(Digit);
        public static Rule Letter = Char(char.IsLetter);
        public static Rule Letters = OneOrMore(Letter);
        public static Rule PlusOrMinus = MatchChar('+') | MatchChar('-');
        public static Rule E = (MatchChar('e') | MatchChar('E')) + Optional(PlusOrMinus);
        public static Rule Exponential = E + Digits;
        public static Rule Integer = Optional(PlusOrMinus) + Digits + Not(MatchChar('.')) + Optional(Exponential);
        public static Rule Float = Optional(PlusOrMinus) + Digits + MatchChar('.') + Digits + Optional(Exponential);
        public static Rule Hexadecimal = Digit | Regex("[a-fA-F]");

        public static Rule MatchAnyString(string input, bool ignoreCase = false) => MatchAnyString(input.Split(' '), ignoreCase);
        public static Rule MatchAnyString(string[] inputs, bool ignoreCase = false) => Or(inputs.Select(s => MatchString(s, ignoreCase)));
        public static Rule MatchEnum<TEnum>() => MatchAnyString(Enum.GetNames(typeof(TEnum)));
    }
}