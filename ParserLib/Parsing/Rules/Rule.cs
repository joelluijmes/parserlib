using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    public abstract class Rule
    {
        private Rule _not;
        private Rule _oneOrMore;
        private Rule _optional;
        private Rule _zeroOrMore;

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

        public Rule Optional => _optional ?? (_optional = Grammar.Optional(this));
        public Rule Not => _not ?? (_not = Grammar.Not(this));
        public Rule OneOrMore => _oneOrMore ?? (_oneOrMore = Grammar.OneOrMore(this));
        public Rule ZeroOrMore => _zeroOrMore ?? (_zeroOrMore = Grammar.ZeroOrMore(this));

        public static Rule operator +(Rule r1, Rule r2) => Grammar.Sequence(r1, r2);
        public static Rule operator |(Rule r1, Rule r2) => Grammar.Or(r1, r2);
        protected internal abstract bool MatchImpl(ParserState state);

        public bool Match(string input)
	    {
		    if (input == null)
			    throw new ArgumentNullException(nameof(input));
			if (input == string.Empty)
				throw new ArgumentException("Input should have content", nameof(input));

		    return MatchImpl(new ParserState(input));
	    }

	    public Node ParseTree(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var state = new ParserState(input);
            if (!MatchImpl(state))
                throw new ParserException($"'{this}' Failed to match '{state.Input}'");

            if (state.Nodes.Count == 1)
                return state.Nodes.First();

            return new Node(ToString(), input, this) {ChildLeafs = state.Nodes};
        }

        public override string ToString() => Name ?? Definition;
        public IEnumerable<Rule> GetChildren() => Children.AsReadOnly();

        protected bool Equals(Rule other)
        {
            return Children.SequenceEqual(other.Children) && string.Equals(Name, other.Name) && string.Equals(Definition, other.Definition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            var other = obj as Rule;
            return (other != null) && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Children?.GetHashCode() ?? 0)*397) ^ ((Name?.GetHashCode() ?? 0)*23) ^ (Definition?.GetHashCode() ?? 0);
            }
        }
    }
}