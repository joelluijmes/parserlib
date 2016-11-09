using System;
using System.Collections.Generic;
using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation
{
    /// <summary>
    ///     Useful extensions for the rule, pretty much just parses the input and than uses the Node Extensions.
    /// </summary>
    public static class RuleExtensions
    {
        /// <summary>
        ///     Processes the specified input using an accumulator.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <returns>T.</returns>
        public static T Process<T>(this Rule rule, string input, Func<T, T, T> accumulator) =>
             rule.ParseTree(input).Process(accumulator);

        /// <summary>
        ///     Processes the specified input using an accumulator.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <returns>T.</returns>
        public static T Process<T>(this ValueRule<T> rule, string input, Func<T, T, T> accumulator) =>
             rule.ParseTree(input).Process(accumulator);

        /// <summary>
        ///     Tries the get value.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if rule matches the input, <c>false</c> otherwise.</returns>
        public static bool TryGetValue<T>(this Rule rule, string input, out T value)
        {
            if (rule.Match(input))
                return rule.ParseTree(input).TryGetValue(out value);

            value = default(T);
            return false;
        }

        /// <summary>
        ///     Tries the get value.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if rule matches the input, <c>false</c> otherwise.</returns>
        public static bool TryGetValue<T>(this ValueRule<T> rule, string input, out T value)
        {
            if (rule.Match(input))
                return rule.ParseTree(input).TryGetValue(out value);

            value = default(T);
            return false;
        }

        /// <summary>
        ///     Finds the first value in the tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>T.</returns>
        public static T FirstValue<T>(this Rule rule, string input) =>
             rule.ParseTree(input).FirstValue<T>();

        /// <summary>
        ///     Finds the first value in the tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>T.</returns>
        public static T FirstValue<T>(this ValueRule<T> rule, string input) =>
             rule.ParseTree(input).FirstValue<T>();

        /// <summary>
        ///     Finds the first value in the tree or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>T.</returns>
        public static T FirstValueOrDefault<T>(this Rule rule, string input) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueOrDefault<T>() : default(T);

        /// <summary>
        ///     Finds the first value in the tree or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>T.</returns>
        public static T FirstValueOrDefault<T>(this ValueRule<T> rule, string input) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueOrDefault<T>() : default(T);

        /// <summary>
        ///     Finds the first value in the tree by the name.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        public static T FirstValueByName<T>(this Rule rule, string input, string name) =>
             rule.ParseTree(input).FirstValueByName<T>(name);

        /// <summary>
        ///     Finds the first value in the tree by the name.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        public static T FirstValueByName<T>(this ValueRule<T> rule, string input, string name) =>
             rule.ParseTree(input).FirstValueByName<T>(name);

        /// <summary>
        ///     Finds the first value in the tree by name or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        public static T FirstValueByNameOrDefault<T>(this Rule rule, string input, string name) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueByNameOrDefault<T>(name) : default(T);

        /// <summary>
        ///     Finds the first value in the tree by name or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        public static T FirstValueByNameOrDefault<T>(this ValueRule<T> rule, string input, string name) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueByNameOrDefault<T>(name) : default(T);

        /// <summary>
        ///     Finds the first value node in the tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        public static ValueNode<T> FirstValueNode<T>(this Rule rule, string input) =>
             rule.ParseTree(input).FirstValueNode<T>();

        /// <summary>
        ///     Finds the first value node in the tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        public static ValueNode<T> FirstValueNode<T>(this ValueRule<T> rule, string input) =>
             rule.ParseTree(input).FirstValueNode<T>();

        /// <summary>
        ///     Finds the first value node in the tree or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        public static ValueNode<T> FirstValueNodeOrDefault<T>(this Rule rule, string input) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueNodeOrDefault<T>() : default(ValueNode<T>);

        /// <summary>
        ///     Finds the first value node in the tree or default.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns>ValueNode&lt;T&gt;.</returns>
        public static ValueNode<T> FirstValueNodeOrDefault<T>(this ValueRule<T> rule, string input) =>
             rule.Match(input) ? rule.ParseTree(input).FirstValueNodeOrDefault<T>() : default(ValueNode<T>);

        /// <summary>
        ///     Determines whether contains value node in matched tree.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns><c>true</c> if contains value node in matched tree; otherwise, <c>false</c>.</returns>
        public static bool ContainsValueNode(this Rule rule, string input) =>
             rule.Match(input) && rule.ParseTree(input).ContainsValueNode();

        /// <summary>
        ///     Determines whether contains value node in matched tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns><c>true</c> if contains value node in matched tree; otherwise, <c>false</c>.</returns>
        public static bool ContainsValueNode<T>(this Rule rule, string input) =>
             rule.Match(input) && rule.ParseTree(input).ContainsValueNode<T>();

        /// <summary>
        ///     Determines whether contains value node in matched tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <param name="input">The input.</param>
        /// <returns><c>true</c> if contains value node in matched tree; otherwise, <c>false</c>.</returns>
        public static bool ContainsValueNode<T>(this ValueRule<T> rule, string input) =>
             rule.Match(input) && rule.ParseTree(input).ContainsValueNode<T>();

        /// <summary>
        ///     Determines whether rule is ValueRule in matched tree.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns><c>true</c> if rule is ValueRule in matched tree; otherwise, <c>false</c>.</returns>
        public static bool IsValueRule(this Rule rule) =>
             Util.IsDerivedFrom(typeof(ValueRule<>), rule.GetType());

        /// <summary>
        ///     Determines whether rule is ValueRule in matched tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <returns><c>true</c> if rule is ValueRule in matched tree; otherwise, <c>false</c>.</returns>
        public static bool IsValueRule<T>(this Rule rule) =>
             Util.IsDerivedFrom(typeof(ValueRule<T>), rule.GetType());

        /// <summary>
        ///     Determines whether rule is ValueRule in matched tree.
        /// </summary>
        /// <typeparam name="T">Strong type of the value.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <returns><c>true</c> if rule is ValueRule in matched tree; otherwise, <c>false</c>.</returns>
        public static bool IsValueRule<T>(this ValueRule<T> rule) =>
             Util.IsDerivedFrom(typeof(ValueRule<T>), rule.GetType());

        /// <summary>
        ///     Return all descendents.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        public static IEnumerable<Rule> Descendents(this Rule rule)
        {
            var leafs = new Stack<Rule>(rule.GetLeafs());
            while (leafs.Any())
            {
                var leaf = leafs.Pop();
                yield return leaf;

                foreach (var l in leaf.GetLeafs())
                    leafs.Push(l);
            }
        }

        /// <summary>
        ///     Return all descendents where the predicate returns true.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        public static IEnumerable<Rule> Descendents(this Rule rule, Predicate<Rule> predicate)
        {
            var leafs = new Queue<Rule>(rule.GetLeafs());
            while (leafs.Any())
            {
                var leaf = leafs.Dequeue();
                if (predicate(leaf))
                {
                    yield return leaf;
                    yield break;
                }

                foreach (var l in leaf.GetLeafs())
                    leafs.Enqueue(l);
            }
        }

        /// <summary>
        ///     Return enumerable of rule, basically returns the current rule and all descendents.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        public static IEnumerable<Rule> AsEnumerable(this Rule rule)
        {
            yield return rule;

            foreach (var descendent in rule.Descendents())
                yield return descendent;
        }
    }
}