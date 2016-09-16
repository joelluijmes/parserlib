using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public sealed class ValueGrammar : Grammar
    {
        public static Rule ConstantValue<T>(string name, T value, Rule rule) => new ConstantValueRule<T>(name, value, rule);
        public static Rule ConvertToValue<T>(string name, Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
        public static Rule ConvertToValue<T>(string name, Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
        public static Rule Accumulate<T>(string name, Func<T, T, T> accumulator, Rule rule) => new EvaluateLeafsRule<T>(name, accumulator, rule);
        public static Rule FirstValue<T>(string name, Rule rule) => new FirstValueRule<T>(name, rule);
        public static Rule Text(string name, Rule rule) => new ConvertToValueRule<string>(name, s => s, rule);

        public static Rule KeyValue<TValue>(KeyValuePair<string, TValue> keyValue) => ConstantValue(keyValue.Key, keyValue.Value, MatchString(keyValue.Key, true));

        public static Rule MatchEnum<TEnum>(string name) => MatchEnum<TEnum, TEnum>(name);

        public static Rule MatchEnum<TEnum, TType>(string name)
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException("TEnum must be an enum");

            var rules = MapEnumToKeyValue<TEnum, TType>().Select(KeyValue);
            return FirstValue<TType>(name, Or(rules));
        }

        private static IEnumerable<KeyValuePair<string, TType>> MapEnumToKeyValue<TEnum, TType>()
        {
            var names = Enum.GetNames(typeof(TEnum));
            Func<string, TType> getValueFunc = s => (TType) Enum.Parse(typeof(TEnum), s);

            return names.Select(n => new KeyValuePair<string, TType>(n, getValueFunc(n)));
        }
    }
}