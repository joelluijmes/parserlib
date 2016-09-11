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
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Children = new List<Rule>(children);
        }

        protected Rule(Rule firstChild)
        {
            if (firstChild == null)
                throw new ArgumentNullException(nameof(firstChild));

            Children = new List<Rule> {firstChild};
        }

        protected Rule FirstChild => Children.First();
        protected List<Rule> Children { get; }
        public string Name { get; set; }
        public abstract string Definition { get; }

        public static Rule operator +(Rule r1, Rule r2) => new SequenceRule(r1, r2);
        public static Rule operator |(Rule r1, Rule r2) => new OrRule(r1, r2);
        protected internal abstract bool MatchImpl(ParserState state);

        public bool Match(string input) => MatchImpl(new ParserState(input));

        public IEnumerable<Node> ParseTree(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var state = new ParserState(input);
            if (!MatchImpl(state))
                throw new ParserException($"'{this}' Failed to match '{state.Input}'");

            return state.Nodes;
        }

        public override string ToString() => Name ?? Definition;
        public IEnumerable<Rule> GetChildren() => Children.AsReadOnly();
    }
}