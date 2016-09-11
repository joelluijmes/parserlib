using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    public sealed class OrRule : Rule
    {
        public OrRule(Rule firstRule, Rule secondRule, params Rule[] rules)
        {
            var allRules = Util.MergeArray(firstRule, secondRule, rules);
            Children.AddRange(allRules.SelectMany(FlattenRules));
        }

        public OrRule(IEnumerable<Rule> rules)
        {
            Children.AddRange(rules.SelectMany(FlattenRules));
        }

        public override string Definition => $"{Children.Skip(1).Aggregate(FirstChild.ToString(), (a, b) => $"{a} | {b}")}";

        protected internal override bool MatchImpl(ParserState state)
        {
            var oldState = state.Clone();
            foreach (var child in Children)
            {
                if (child.MatchImpl(state))
                    return true;

                state.Assign(oldState);
            }

            return false;
        }

        public override string ToString() => $"({base.ToString()})";

        private static IEnumerable<Rule> FlattenRules(Rule r) => r is OrRule ? r.GetChildren() : new[] {r};
    }
}