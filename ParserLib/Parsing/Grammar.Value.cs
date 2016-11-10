using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Provides base class for collection of rules
    /// </summary>
    public abstract partial class Grammar
    {
        /// <summary>
        ///     Converts a matched <see cref="Integer" /> to binary representation with a padding which also truncates the binary
        ///     string.
        /// </summary>
        /// <param name="padding">The padding, if zero returns the binary string without extra padding or truncating.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> Binary(int padding = 0) => ConvertToValue(str => BinaryConverter(padding, str), Integer);

        /// <summary>
        ///     Converts a matched <see cref="Integer" /> to binary representation with a padding which also truncates the binary
        ///     string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="padding">The padding, if zero returns the binary string without extra padding or truncating.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> Binary(string name, int padding = 0) => ConvertToValue(name, str => BinaryConverter(padding, str), Integer);

        /// <summary>
        ///     Converts a matched <see cref="ValueRule{T}" /> of <see cref="int" />, to binary representation with a padding which
        ///     also truncates the binary string.
        /// </summary>
        /// <param name="rule">The valuerule</param>
        /// <param name="padding">The padding, if zero returns the binary string without extra padding or truncating.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> ConvertBinary(ValueRule<int> rule, int padding = 0) => ConvertToValue(node => BinaryConverter(padding, node.FirstValue<int>()), rule);

        /// <summary>
        ///     Converts a matched <see cref="ValueRule{T}" /> of <see cref="int" />, to binary representation with a padding which
        ///     also truncates the binary string.
        /// </summary>
        /// <param name="name">The namae</param>
        /// <param name="rule">The valuerule</param>
        /// <param name="padding">The padding, if zero returns the binary string without extra padding or truncating.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> ConvertBinary(string name, ValueRule<int> rule, int padding = 0) => ConvertToValue(node => BinaryConverter(padding, node.FirstValue<int>()), rule);

        /// <summary>
        ///     Creates a rule that validates a parsed value in a specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Range<T>(T minimum, T maximum, Rule rule) => new RangeValueRule<T>(minimum, maximum, rule);

        /// <summary>
        ///     Creates a rule that validates a parsed value in a specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Range<T>(T minimum, T maximum, IComparer<T> comparer, Rule rule) => new RangeValueRule<T>(minimum, maximum, comparer, rule);

        /// <summary>
        ///     Creates a rule that validates a parsed value in a specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Range<T>(string name, T minimum, T maximum, Rule rule) => new RangeValueRule<T>(name, minimum, maximum, rule);

        /// <summary>
        ///     Creates a rule that validates a parsed value in a specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Range<T>(string name, T minimum, T maximum, IComparer<T> comparer, Rule rule) => new RangeValueRule<T>(name, minimum, maximum, comparer, rule);

        /// <summary>
        ///     If the rule matches return constant value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConstantValue<T>(T value, Rule rule) => new ConstantValueRule<T>(value, rule);

        /// <summary>
        ///     If the rule matches return constant value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="value">The value.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConstantValue<T>(string name, T value, Rule rule) => new ConstantValueRule<T>(name, value, rule);

        /// <summary>
        ///     Converts the matched string to value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConvertToValue<T>(Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(valueFunc, rule);

        /// <summary>
        ///     Converts the matched string to value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConvertToValue<T>(string name, Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);

        /// <summary>
        ///     Converts the matched node to value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConvertToValue<T>(Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(valueFunc, rule);

        /// <summary>
        ///     Converts the matched node to value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> ConvertToValue<T>(string name, Func<Node, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);

        /// <summary>
        ///     Accumulates the child values.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Accumulate<T>(Func<T, T, T> accumulator, Rule rule) => new AccumulateRule<T>(accumulator, rule);

        /// <summary>
        ///     Accumulates the child values.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> Accumulate<T>(string name, Func<T, T, T> accumulator, Rule rule) => new AccumulateRule<T>(name, accumulator, rule);

        /// <summary>
        ///     First value of the matched rule.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> FirstValue<T>(Rule rule) => new FirstValueRule<T>(rule);

        /// <summary>
        ///     First value of the matched rule.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;T&gt;.</returns>
        public static ValueRule<T> FirstValue<T>(string name, Rule rule) => new FirstValueRule<T>(name, rule);

        /// <summary>
        ///     Returns the matched string.
        /// </summary>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> Text(Rule rule) => new ConvertToValueRule<string>(s => s, rule);

        /// <summary>
        ///     Returns the matched string.
        /// </summary>
        /// <param name="name">The name given to the rule and node.</param>
        /// <param name="rule">The rule to give value.</param>
        /// <returns>ValueRule&lt;System.String&gt;.</returns>
        public static ValueRule<string> Text(string name, Rule rule) => new ConvertToValueRule<string>(name, s => s, rule);

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a byte.
        /// </summary>
        /// <returns>ValueRule&lt;System.Byte&gt;.</returns>
        public static ValueRule<byte> Int8() => FirstValue<byte>(ConvertToValue("hex", HexConverterByte, HexNumber) | ConvertToValue("dec", byte.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a byte.
        /// </summary>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;System.Byte&gt;.</returns>
        public static ValueRule<byte> Int8(string name) => FirstValue<byte>(name, ConvertToValue("hex", HexConverterByte, HexNumber) | ConvertToValue("dec", byte.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a short.
        /// </summary>
        /// <returns>ValueRule&lt;System.Int16&gt;.</returns>
        public static ValueRule<short> Int16() => FirstValue<short>(ConvertToValue("hex", HexConverterShort, HexNumber) | ConvertToValue("dec", short.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a short.
        /// </summary>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;System.Int16&gt;.</returns>
        public static ValueRule<short> Int16(string name) => FirstValue<short>(name, ConvertToValue("hex", HexConverterShort, HexNumber) | ConvertToValue("dec", short.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a int.
        /// </summary>
        /// <returns>ValueRule&lt;System.Int32&gt;.</returns>
        public static ValueRule<int> Int32() => FirstValue<int>(ConvertToValue("hex", HexConverterInt, HexNumber) | ConvertToValue("dec", int.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a int.
        /// </summary>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;System.Int32&gt;.</returns>
        public static ValueRule<int> Int32(string name) => FirstValue<int>(name, ConvertToValue("hex", HexConverterInt, HexNumber) | ConvertToValue("dec", int.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a long.
        /// </summary>
        /// <returns>ValueRule&lt;System.Int64&gt;.</returns>
        public static ValueRule<long> Int64() => FirstValue<long>(ConvertToValue("hex", HexConverterLong, HexNumber) | ConvertToValue("dec", long.Parse, Integer));

        /// <summary>
        ///     Converts the matched string (in hex or decimal) to a long.
        /// </summary>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;System.Int64&gt;.</returns>
        public static ValueRule<long> Int64(string name) => FirstValue<long>(name, ConvertToValue("hex", HexConverterLong, HexNumber) | ConvertToValue("dec", long.Parse, Integer));

        /// <summary>
        ///     Convert a specific key to value. Useful for enums for example.
        /// </summary>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="keyValue">The key value.</param>
        /// <param name="ignoreCase">Make key case sensitive.</param>
        /// <returns>ValueRule&lt;TValue&gt;.</returns>
        public static ValueRule<TValue> KeyValue<TValue>(KeyValuePair<string, TValue> keyValue, bool ignoreCase = true) => ConstantValue(keyValue.Key, keyValue.Value, MatchString(keyValue.Key, ignoreCase));

        /// <summary>
        ///     Converts the Enum to value specified in the enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <typeparam name="TType">The type of the t type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="ignoreCase">Make key case sensitive.</param>
        /// <returns>ValueRule&lt;TType&gt;.</returns>
        public static ValueRule<TType> EnumValue<TEnum, TType>(TEnum type, bool ignoreCase = true) => ConstantValue(typeof(TEnum).Name, (TType) (object) type, MatchString(type.ToString(), ignoreCase));

        /// <summary>
        ///     Converts the Enum to value specified in the enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;TEnum&gt;.</returns>
        public static ValueRule<TEnum> EnumValue<TEnum>(string name = null) => EnumValue<TEnum, TEnum>();

        /// <summary>
        ///     Converts the Enum to value specified in the enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <typeparam name="TType">The type of the t type.</typeparam>
        /// <param name="name">The name given to the rule and node.</param>
        /// <returns>ValueRule&lt;TType&gt;.</returns>
        /// <exception cref="System.ArgumentException">TEnum must be an enum</exception>
        public static ValueRule<TType> EnumValue<TEnum, TType>(string name = null)
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException("TEnum must be an enum");

            var rules = MapEnumToKeyValue<TEnum, TType>().Select(e => KeyValue(e));
            return FirstValue<TType>(name, Or(rules));
        }

        private static IEnumerable<KeyValuePair<string, TType>> MapEnumToKeyValue<TEnum, TType>()
        {
            var names = Enum.GetNames(typeof(TEnum));
            Func<string, TType> getValueFunc = s => (TType) Enum.Parse(typeof(TEnum), s);

            return names.Select(n => new KeyValuePair<string, TType>(n, getValueFunc(n)));
        }

        private static byte HexConverterByte(string matched)
        {
            matched = matched.Trim('h');
            return Convert.ToByte(matched, 16);
        }

        private static short HexConverterShort(string matched)
        {
            matched = matched.Trim('h');
            return Convert.ToInt16(matched, 16);
        }

        private static int HexConverterInt(string matched)
        {
            matched = matched.Trim('h');
            return Convert.ToInt32(matched, 16);
        }

        private static long HexConverterLong(string matched)
        {
            matched = matched.Trim('h');
            return Convert.ToInt64(matched, 16);
        }

        private static string BinaryConverter(int padding, int value)
        {
            if (padding == 0)
                return Convert.ToString(value, 2);
            
            var binary = Convert.ToString(value, 2).PadLeft(padding, '0');
            var len = Math.Min(padding, binary.Length);

            return binary.Substring(binary.Length - len, len);
        }

        private static string BinaryConverter(int padding, string str) => BinaryConverter(padding, int.Parse(str));
    }
}
