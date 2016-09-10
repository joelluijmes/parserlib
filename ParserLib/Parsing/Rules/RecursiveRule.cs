using System;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    public sealed class RecursiveRule : Rule
    {
        public RecursiveRule(Func<Rule> ruleFunc)
        {
            RuleFunc = ruleFunc;
        }

        public Func<Rule> RuleFunc { get; }

        public override string Definition => RuleFunc().Definition;
        protected internal override bool MatchImpl(ParserState state)
        {
            if (!Children.Any())
                Children.Add(RuleFunc());

            return FirstChild.MatchImpl(state);
        }

        public override string ToString() => Name ?? "recursive";
    }
}