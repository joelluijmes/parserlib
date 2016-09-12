using System;
using System.Collections.Generic;

namespace ParserLib.Parsing.Rules
{
    public class NodeRule : Rule
    {
        public NodeRule(string name, Rule rule) : base(rule)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public override string Definition => FirstChild.Definition;

        protected internal override bool MatchImpl(ParserState state)
        {
            var node = CreateNode(Name, state.Input, state.Position);
            var oldChilds = state.Nodes;
            state.Nodes = new List<Node>();

            if (!FirstChild.MatchImpl(state))
            {
                state.Nodes = oldChilds;
                return false;
            }

            node.End = state.Position;
            node.Leafs = state.Nodes;

            oldChilds.Add(node);
            state.Nodes = oldChilds;
            return true;
        }

        protected virtual Node CreateNode(string name, string input, int begin) => new Node(name, input, begin);
    }
}