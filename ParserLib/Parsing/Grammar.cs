using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public abstract class Grammar
    {
        private static readonly Rule _endRule = new EndRule();
        private static readonly Rule _startRule = new StartRule();

        public static Rule Node(string name, Rule rule) => new NodeRule(name, rule);
        public static Rule MatchString(string pattern, bool ignoreCase = false) => new StringRule(pattern, ignoreCase);
        public static Rule Not(Rule rule) => new NotRule(rule);
        public static Rule Sequence(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new SequenceRule(firstRule, secondRule, moreRules);
        public static Rule Sequence(IEnumerable<Rule> rules) => new SequenceRule(rules);
        public static Rule Or(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new OrRule(firstRule, secondRule, moreRules);
        public static Rule Or(IEnumerable<Rule> rules) => new OrRule(rules);
        public static Rule Start() => _startRule;
        public static Rule End() => _endRule;
        public static Rule OneOrMore(Rule rule) => new OneOrMoreRule(rule);
        public static Rule ZeroOrMore(Rule rule) => new ZeroOrMoreRule(rule);
        public static Rule Optional(Rule rule) => new OptionalRule(rule);
        public static Rule Func(Func<Rule> ruleFunc) => new FuncRule(ruleFunc);
        public static Rule Regex(Regex regex) => new RegexRule(regex);
        public static Rule Regex(string pattern) => new RegexRule(pattern);
        public static Rule Char(Predicate<char> predicate) => new CharRule(predicate);
        public static Rule MatchChar(char c) => new CharRule(x => x == c) {Name = $"'{c}'"};
        public static Rule Value<T>(string name, T value, Rule rule) => new ConstantValueRule<T>(name, value, rule);
        public static Rule ConvertToValue<T>(string name, Func<string, T> valueFunc, Rule rule) => new ConvertToValueRule<T>(name, valueFunc, rule);

        public static Rule Binary(Rule left, Rule op, Rule right, bool fixedOrder = false) => fixedOrder
            ? left + op + right
            : (left + op + right) | (right + op + left);
    }
}