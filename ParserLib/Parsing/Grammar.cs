using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
	public abstract partial class Grammar
	{
		private static readonly Rule _endRule = new EndRule();
		private static readonly Rule _startRule = new StartRule();

		public static Rule Node(Rule rule) => new NodeRule(rule);
		public static Rule Node(string name, Rule rule) => new NodeRule(name, rule);
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
		public static Rule MatchAnyChar() => Char(i => true);
		public static Rule MatchAnyChar(char c, params char[] chars) => Or(Util.MergeArray(c, chars).Select(r => MatchChar(r)));
		public static Rule MatchString(string pattern, bool ignoreCase = false) => new StringRule(pattern, ignoreCase);
		public static Rule MatchAnyString(string input, bool ignoreCase = false) => MatchAnyString(input.Split(' '), ignoreCase);
		public static Rule MatchAnyString(string[] inputs, bool ignoreCase = false) => Or(inputs.Select(s => MatchString(s, ignoreCase)));
		public static Rule MatchWhile(Rule rule) => OneOrMore(End().Not + rule);
		public static Rule MatchEnum<TEnum>() => MatchAnyString(Enum.GetNames(typeof(TEnum)));

		public static Rule MatchChar(char c, bool ignoreCase = false)
		{
			var rule = new CharRule(x => ignoreCase
				? char.ToLower(x) == char.ToLower(c)
				: x == c) {Name = $"'{c}'"};

			return rule;
		}

		public static Rule Binary(Rule left, Rule op, Rule right, bool fixedOrder = false) => fixedOrder
			? left + op + right
			: (left + op + right) | (right + op + left);
	}
}