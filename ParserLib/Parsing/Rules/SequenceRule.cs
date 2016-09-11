using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserLib.Parsing.Rules
{
    public sealed class SequenceRule : Rule
    {
        public SequenceRule(Rule firstRule, Rule secondRule, params Rule[] rules)
        {
            var allRules = Util.MergeArray(firstRule, secondRule, rules);
            Children.AddRange(allRules.SelectMany(FlattenRules));
        }

        public SequenceRule(IEnumerable<Rule> rules)
        {
            Children.AddRange(rules.SelectMany(FlattenRules));
        }

        public override string Definition => Children.Skip(1).Aggregate(FirstChild.ToString(), (a, b) => $"{a} + {b}");

        protected internal override bool MatchImpl(ParserState state)
        {
            var oldState = state.Clone();
            if (Children.All(c => c.MatchImpl(state)))
                return true;

            state.Assign(oldState);
            return false;
        }

        public override string ToString() => $"({base.ToString()})";

        private static IEnumerable<Rule> FlattenRules(Rule r) => r is SequenceRule ? r.GetChildren() : new[] { r };
    }
}