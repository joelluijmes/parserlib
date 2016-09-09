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
            Children = new List<Rule>();
            Children.Add(firstChild);
        }

        public string Name { get; set; }

        public Rule FirstChild => Children.First();
        public IList<Rule> Children { get; }
        public abstract string Definition { get; }

        public static Rule operator +(Rule r1, Rule r2) => new SequenceRule(r1, r2);
        public static Rule operator |(Rule r1, Rule r2) => new OrRule(r1, r2);
        protected internal abstract bool MatchImpl(ParserState state);

        public bool Match(string input) => MatchImpl(new ParserState(input, 0));

        public IEnumerable<Node> ParseTree(string input)
        {
            var state = new ParserState(input, 0);
            if (!MatchImpl(state))
                throw new NotImplementedException();

            return state.Childs;
        }

        public override string ToString() => Name ?? Definition;
    }
}