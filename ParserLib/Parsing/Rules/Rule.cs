using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    public abstract class Rule
    {
        protected Rule()
        {
            Children = new List<Rule>();
        }

        protected Rule(IEnumerable<Rule> children)
        {
            Children = new List<Rule>(children);
        }

        protected Rule(Rule firstChild)
        {
            Children = new List<Rule> {firstChild};
        }

        protected Rule FirstChild => Children.First();
        protected IList<Rule> Children { get; }
        public string Name { get; set; }
        public abstract string Definition { get; }

        public static Rule operator +(Rule r1, Rule r2) => new SequenceRule(r1, r2);
        public static Rule operator |(Rule r1, Rule r2) => new OrRule(r1, r2);
        protected internal abstract bool MatchImpl(ParserState state);

        public bool Match(string input) => MatchImpl(new ParserState(input));

        public IEnumerable<Node> ParseTree(string input)
        {
            var state = new ParserState(input);
            if (!MatchImpl(state))
                throw new NotImplementedException();

            return state.Nodes;
        }

        public override string ToString() => Name ?? Definition;
    }
}