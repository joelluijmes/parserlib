using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    [DebuggerStepThrough]
    public sealed class ParserState
    {
		private readonly Dictionary<int, Dictionary<Rule, Node>> _cache = new Dictionary<int, Dictionary<Rule, Node>>();

		public ParserState(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            Input = input;
            Position = 0;
            Nodes = new List<Node>();
        }

        public string Input { get; private set; }
        public int Position { get; set; }
        public IList<Node> Nodes { get; set; }

        public ParserState Clone() => new ParserState(Input)
        {
            Position = Position,
            Nodes = Nodes.ToList()
        };

        public void Assign(ParserState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Input = state.Input;
            Position = state.Position;
            Nodes = state.Nodes.ToList();
        }

		public void StoreCache(Rule rule, int pos, Node node)
		{
			if (!_cache.ContainsKey(pos))
				_cache.Add(pos, new Dictionary<Rule, Node>());

			var tmp = _cache[pos];
			if (!tmp.ContainsKey(rule))
				tmp.Add(rule, node);
		}

		public bool TryGetCache(NodeRule rule, out Node node)
		{
			node = null;
			if (!_cache.ContainsKey(Position))
				return false;
			if (!_cache[Position].ContainsKey(rule))
				return false;

			node = _cache[Position][rule];
			return true;
		}
	}
}