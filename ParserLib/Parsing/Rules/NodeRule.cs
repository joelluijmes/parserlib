using System.Collections.Generic;

namespace ParserLib.Parsing.Rules
{
    public sealed class NodeRule : Rule
    {
        public NodeRule(string name, Rule rule) : base(rule)
        {
            Name = name;
        }

        public override string Definition => FirstChild.Definition;

        protected internal override bool MatchImpl(ParserState state)
        {
            var node = new Node(Name, state.Input, state.Position);
            var oldChilds = state.Nodes;
            state.Nodes = new List<Node>();

            if (!FirstChild.MatchImpl(state))
            {
                state.Nodes = oldChilds;
                return false;
            }

            node.End = state.Position;
            node.Childs = state.Nodes;

            oldChilds.Add(node);
            state.Nodes = oldChilds;
            return true;
        }
    }
}