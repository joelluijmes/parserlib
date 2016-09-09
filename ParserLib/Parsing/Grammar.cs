using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public abstract class Grammar
    {
        private static readonly Rule _endRule = new EndRule();
        private static readonly Rule _startRule = new StartRule();

        public static Rule Node(string name, Rule rule) => new NodeRule(name, rule);
        public static Rule MatchString(string pattern) => new StringRule(pattern);
        public static Rule Not(Rule rule) => new NotRule(rule);
        public static Rule Sequence(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new SequenceRule(firstRule, secondRule, moreRules);
        public static Rule Or(Rule firstRule, Rule secondRule, params Rule[] moreRules) => new OrRule(firstRule, secondRule, moreRules);
        public static Rule Start() => _startRule;
        public static Rule End() => _endRule;
        public static Rule OneOrMore(Rule rule) => new OneOrMoreRule(rule);
        public static Rule ZeroOrMore(Rule rule) => new ZeroOrMoreRule(rule);
        public static Rule Optional(Rule rule) => new OptionalRule(rule);
        public static Rule Regex(Regex regex) => new RegexRule(regex);
        public static Rule Regex(string pattern) => new RegexRule(pattern);

        protected abstract IEnumerable<Rule> GetGrammarRules();
    }
}