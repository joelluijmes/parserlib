using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Provides base class for collection of rules
    /// </summary>
    public abstract partial class Grammar
    {
        private static readonly Rule _endRule = new EndRule();
        private static readonly Rule _startRule = new StartRule();

        /// <summary>
        ///     Nodes the specified rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule Node(Rule rule) => new NodeRule(rule);

        /// <summary>
        ///     Nodes the specified rule with a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule Node(string name, Rule rule) => new NodeRule(name, rule);

        /// <summary>
        ///     Nots the specified rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule Not(Rule rule) => new NotRule(rule);

        /// <summary>
        ///     Sequences the specified rules.
        /// </summary>
        /// <param name="firstRule">The first rule.</param>
        /// <param name="secondRule">The second rule.</param>
        /// <param name="moreRules">The more rules.</param>
        /// <returns>Rule.</returns>
        public static Rule Sequence(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new SequenceRule(firstRule, secondRule, moreRules);

        /// <summary>
        ///     Sequences the collection of rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>Rule.</returns>
        public static Rule Sequence(IEnumerable<Rule> rules) => new SequenceRule(rules);

        /// <summary>
        ///     Ors the specified rules.
        /// </summary>
        /// <param name="firstRule">The first rule.</param>
        /// <param name="secondRule">The second rule.</param>
        /// <param name="moreRules">The more rules.</param>
        /// <returns>Rule.</returns>
        public static Rule Or(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new OrRule(firstRule, secondRule, moreRules);

        /// <summary>
        ///     Ors the collection rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>Rule.</returns>
        public static Rule Or(IEnumerable<Rule> rules) => new OrRule(rules);

        /// <summary>
        ///     Start Rule, matches when its at the first position
        /// </summary>
        /// <returns>Rule.</returns>
        public static Rule Start() => _startRule;

        /// <summary>
        ///     End Rule, matches when end of input has been reached
        /// </summary>
        /// <returns>Rule.</returns>
        public static Rule End() => _endRule;

        /// <summary>
        ///     One or more, matching this rule
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule OneOrMore(Rule rule) => new OneOrMoreRule(rule);

        /// <summary>
        ///     Zero or more, matching this rule
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule ZeroOrMore(Rule rule) => new ZeroOrMoreRule(rule);

        /// <summary>
        ///     Make specified rule optional
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule Optional(Rule rule) => new OptionalRule(rule);

        /// <summary>
        ///     Makes a Func of the specified rule. Used for pointing to rules (i.e. recursive.)
        /// </summary>
        /// <param name="ruleFunc">The Func that return the rule.</param>
        /// <returns>Rule.</returns>
        public static Rule Func(Func<Rule> ruleFunc) => new FuncRule(ruleFunc);

        /// <summary>
        ///     Matches the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns>Rule.</returns>
        public static Rule Regex(Regex regex) => new RegexRule(regex);

        /// <summary>
        ///     Matches the specified regex pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>Rule.</returns>
        public static Rule Regex(string pattern) => new RegexRule(pattern);

        /// <summary>
        ///     Matches if the predicate of char returns true.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Rule.</returns>
        public static Rule Char(Predicate<char> predicate) => new CharRule(predicate);

        /// <summary>
        ///     Matches any character.
        /// </summary>
        /// <returns>Rule.</returns>
        public static Rule MatchAnyChar() => Char(i => true);

        /// <summary>
        ///     Matches any of the specified characters.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="chars">The other characters.</param>
        /// <returns>Rule.</returns>
        public static Rule MatchAnyChar(char c, params char[] chars) => Or(Util.MergeArray(c, chars).Select(r => MatchChar(r)));

        /// <summary>
        ///     Matches the string.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Rule.</returns>
        public static Rule MatchString(string pattern, bool ignoreCase = false) => new StringRule(pattern, ignoreCase);

        /// <summary>
        ///     Matches any of the strings seperated by a <c>,</c>
        /// </summary>
        /// <param name="input">The input strings.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Rule.</returns>
        public static Rule MatchAnyString(string input, bool ignoreCase = false) => MatchAnyString(input.Split(' '), ignoreCase);

        /// <summary>
        ///     Matches any of the specified strings.
        /// </summary>
        /// <param name="inputs">The inputs strings.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Rule.</returns>
        public static Rule MatchAnyString(string[] inputs, bool ignoreCase = false) => Or(inputs.Select(s => MatchString(s, ignoreCase)));

        /// <summary>
        ///     Matches the while specified rule matches and end of string hasn't been reached.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>Rule.</returns>
        public static Rule MatchWhile(Rule rule) => OneOrMore(End().Not + rule);

        /// <summary>
        ///     Matches any the enum names.
        /// </summary>
        /// <typeparam name="TEnum">The enum.</typeparam>
        /// <returns>Rule.</returns>
        public static Rule MatchEnum<TEnum>() => MatchAnyString(Enum.GetNames(typeof(TEnum)));

        /// <summary>
        ///     Matches the character.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Rule.</returns>
        public static Rule MatchChar(char c, bool ignoreCase = false)
        {
            var rule = new CharRule(x => ignoreCase
                ? char.ToLower(x) == char.ToLower(c)
                : x == c) {Name = $"'{c}'"};

            return rule;
        }

        /// <summary>
        ///     Matches if left + op + right. Useful if operands are commutative.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="op">The operand.</param>
        /// <param name="right">The right.</param>
        /// <param name="commutative">if set to <c>true</c> [right + op + left] is also valid.</param>
        /// <returns>Rule.</returns>
        public static Rule Binary(Rule left, Rule op, Rule right, bool commutative = true) => commutative
            ? (left + op + right) | (right + op + left)
            : left + op + right;
    }
}