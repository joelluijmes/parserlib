using System;
using ParserLib.Evaluation.Nodes;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation
{
    public static class RuleExtensions
    {
        public static T Process<T>(this Rule rule, string input, Func<T, T, T> accumulator)
            => rule.ParseTree(input).Process(accumulator);

        public static T Process<T>(this ValueRule<T> rule, string input, Func<T, T, T> accumulator)
            => rule.ParseTree(input).Process(accumulator);

        public static bool TryGetValue<T>(this Rule rule, string input, out T value)
            => rule.ParseTree(input).TryGetValue(out value);

        public static bool TryGetValue<T>(this ValueRule<T> rule, string input, out T value)
            => rule.ParseTree(input).TryGetValue(out value);

        public static T FirstValue<T>(this Rule rule, string input)
            => rule.ParseTree(input).FirstValue<T>();

        public static T FirstValue<T>(this ValueRule<T> rule, string input)
            => rule.ParseTree(input).FirstValue<T>();

        public static T FirstValueOrDefault<T>(this Rule rule, string input)
            => rule.ParseTree(input).FirstValueOrDefault<T>();

        public static T FirstValueOrDefault<T>(this ValueRule<T> rule, string input)
            => rule.ParseTree(input).FirstValueOrDefault<T>();

        public static T FirstValueByName<T>(this Rule rule, string input, string name)
            => rule.ParseTree(input).FirstValueByName<T>(name);

        public static T FirstValueByName<T>(this ValueRule<T> rule, string input, string name)
            => rule.ParseTree(input).FirstValueByName<T>(name);

        public static T FirstValueByNameOrDefault<T>(this Rule rule, string input, string name)
            => rule.ParseTree(input).FirstValueByNameOrDefault<T>(name);

        public static T FirstValueByNameOrDefault<T>(this ValueRule<T> rule, string input, string name)
            => rule.ParseTree(input).FirstValueByNameOrDefault<T>(name);

        public static ValueNode<T> FirstValueNode<T>(this Rule rule, string input)
            => rule.ParseTree(input).FirstValueNode<T>();

        public static ValueNode<T> FirstValueNode<T>(this ValueRule<T> rule, string input)
            => rule.ParseTree(input).FirstValueNode<T>();

        public static ValueNode<T> FirstValueNodeOrDefault<T>(this Rule rule, string input)
            => rule.ParseTree(input).FirstValueNodeOrDefault<T>();

        public static ValueNode<T> FirstValueNodeOrDefault<T>(this ValueRule<T> rule, string input)
            => rule.ParseTree(input).FirstValueNodeOrDefault<T>();

        public static bool ContainsValueNode(this Rule rule, string input)
            => rule.ParseTree(input).ContainsValueNode();

        public static bool ContainsValueNode<T>(this Rule rule, string input)
            => rule.ParseTree(input).ContainsValueNode<T>();

        public static bool ContainsValueNode<T>(this ValueRule<T> rule, string input)
            => rule.ParseTree(input).ContainsValueNode<T>();

        public static bool IsValueRule(this Rule rule, string input)
            => Util.IsDerivedFrom(typeof(ValueRule<>), rule.GetType());

        public static bool IsValueRule<T>(this Rule rule, string input)
            => Util.IsDerivedFrom(typeof(ValueRule<T>), rule.GetType());

        public static bool IsValueRule<T>(this ValueRule<T> rule, string input)
            => Util.IsDerivedFrom(typeof(ValueRule<T>), rule.GetType());
    }
}