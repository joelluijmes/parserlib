using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserLib.Parsing.Rules
{
    public sealed class OrRule : Rule
    {
        public OrRule(Rule firstRule, Rule secondRule, params Rule[] rules)
            : base(Util.MergeArray(firstRule, secondRule, rules))
        {
        }

        public OrRule(IEnumerable<Rule> rules)
            : base(rules)
        {
        }

        public override string Definition
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(FirstChild);

                if ((Children.Count == 2) && Children[1] is SequenceRule)
                    stringBuilder.Append($" | {Children[1].Definition}");
                else
                    foreach (var child in Children.Skip(1))
                        stringBuilder.Append($" | {child}");

                return stringBuilder.ToString();
            }
        }

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
    }
}