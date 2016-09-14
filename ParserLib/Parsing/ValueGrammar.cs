using System;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public sealed class ValueGrammar : Grammar
    {
        public static Rule ConstantValue<T>(string name, T value, Rule rule) => new ConstantValueRule<T>(name, value, rule);
        public static Rule ConvertToValue<T>(string name, Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
        public static Rule ConvertToValue<T>(string name, Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
    }
}