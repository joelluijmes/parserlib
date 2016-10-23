using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public abstract partial class Grammar
    {
        public static ValueRule<T> ConstantValue<T>(T value, Rule rule) => new ConstantValueRule<T>(value, rule);
        public static ValueRule<T> ConstantValue<T>(string name, T value, Rule rule) => new ConstantValueRule<T>(name, value, rule);
        public static ValueRule<T> ConvertToValue<T>(Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(valueFunc, rule);
        public static ValueRule<T> ConvertToValue<T>(string name, Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
        public static ValueRule<T> ConvertToValue<T>(Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(valueFunc, rule);
        public static ValueRule<T> ConvertToValue<T>(string name, Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);
        public static ValueRule<T> Accumulate<T>(Func<T, T, T> accumulator, Rule rule) => new AccumulateRule<T>(accumulator, rule);
        public static ValueRule<T> Accumulate<T>(string name, Func<T, T, T> accumulator, Rule rule) => new AccumulateRule<T>(name, accumulator, rule);
        public static ValueRule<T> FirstValue<T>(Rule rule) => new FirstValueRule<T>(rule);
        public static ValueRule<T> FirstValue<T>(string name, Rule rule) => new FirstValueRule<T>(name, rule);
        public static ValueRule<string> Text(Rule rule) => new ConvertToValueRule<string>(s => s, rule);
        public static ValueRule<string> Text(string name, Rule rule) => new ConvertToValueRule<string>(name, s => s, rule);
        public static ValueRule<byte> Int8() => FirstValue<byte>(ConvertToValue("hex", s => Convert.ToByte(s, 16), HexNumber) | ConvertToValue("dec", byte.Parse, Integer));
        public static ValueRule<byte> Int8(string name) => FirstValue<byte>(name, ConvertToValue("hex", s => Convert.ToByte(s, 16), HexNumber) | ConvertToValue("dec", byte.Parse, Integer));
        public static ValueRule<short> Int16() => FirstValue<short>(ConvertToValue("hex", s => Convert.ToInt16(s, 16), HexNumber) | ConvertToValue("dec", short.Parse, Integer));
        public static ValueRule<short> Int16(string name) => FirstValue<short>(name, ConvertToValue("hex", s => Convert.ToInt16(s, 16), HexNumber) | ConvertToValue("dec", short.Parse, Integer));
        public static ValueRule<int> Int32() => FirstValue<int>(ConvertToValue("hex", s => Convert.ToInt32(s, 16), HexNumber) | ConvertToValue("dec", int.Parse, Integer));
        public static ValueRule<int> Int32(string name) => FirstValue<int>(name, ConvertToValue("hex", s => Convert.ToInt32(s, 16), HexNumber) | ConvertToValue("dec", int.Parse, Integer));
        public static ValueRule<long> Int64() => FirstValue<long>(ConvertToValue("hex", s => Convert.ToInt64(s, 16), HexNumber) | ConvertToValue("dec", long.Parse, Integer));
        public static ValueRule<long> Int64(string name) => FirstValue<long>(name, ConvertToValue("hex", s => Convert.ToInt64(s, 16), HexNumber) | ConvertToValue("dec", long.Parse, Integer));

        public static ValueRule<TValue> KeyValue<TValue>(KeyValuePair<string, TValue> keyValue) => ConstantValue(keyValue.Key, keyValue.Value, MatchString(keyValue.Key, true));
        public static ValueRule<TType> EnumValue<TEnum, TType>(TEnum type) => ConstantValue(typeof(TEnum).Name, (TType) (object) type, MatchString(type.ToString(), true));
        public static ValueRule<TEnum> EnumValue<TEnum>(string name = null) => EnumValue<TEnum, TEnum>();

        public static ValueRule<TType> EnumValue<TEnum, TType>(string name = null)
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