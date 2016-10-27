using System.Collections.Generic;

namespace ParserLib.Parsing.Rules
{
    public class NodeRule : Rule
    {
        public NodeRule(Rule rule) : base(rule)
        {
        }

        public NodeRule(string name, Rule rule) : base(rule)
        {
            Name = name;
        }

        public override string Definition => FirstChild.Definition;

        protected internal override bool MatchImpl(ParserState state)
        {
	        Node node;
			if (state.TryGetCache(this, out node))
	        {
		        if (node == null)
			        return false;

		        state.Position = node.End;
				state.Nodes.Add(node);
		        return true;
	        }

            node = CreateNode(Name, state.Input, state.Position, this);
            var oldChilds = state.Nodes;
            state.Nodes = new List<Node>();

	        var oldPosition = state.Position;
            if (FirstChild.MatchImpl(state))
            {
				node.End = state.Position;
				node.ChildLeafs = state.Nodes;

				oldChilds.Add(node);
				state.Nodes = oldChilds;
				state.StoreCache(this, oldPosition, node);
				return true;
			}

			state.Nodes = oldChilds;
			state.StoreCache(this, oldPosition, null);
			return false;
        }

        protected virtual Node CreateNode(string name, string input, int begin, Rule matchedRule) => new Node(name, input, begin, matchedRule);
    }
}