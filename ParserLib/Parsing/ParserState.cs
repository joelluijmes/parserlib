using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Model for holding state of parsing. This class cannot be inherited.
    /// </summary>
    [DebuggerStepThrough]
    public sealed class ParserState
    {
        private Dictionary<int, Dictionary<Rule, Node>> _cache = new Dictionary<int, Dictionary<Rule, Node>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserState" /> class.
        /// </summary>
        /// <param name="input">The input which is parsed.</param>
        /// <exception cref="System.ArgumentNullException">input</exception>
        public ParserState(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            Input = input;
            Position = 0;
            Nodes = new List<Node>();
        }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        /// <value>The input.</value>
        public string Input { get; private set; }

        /// <summary>
        ///     Gets or sets the current position of parsing.
        /// </summary>
        /// <value>The position.</value>
        public int Position { get; set; }

        /// <summary>
        ///     Gets or sets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        public IList<Node> Nodes { get; set; }

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns>ParserState.</returns>
        public ParserState Clone() => new ParserState(Input)
        {
            Position = Position,
            Nodes = Nodes.ToList(),
            _cache = _cache
        };

        /// <summary>
        ///     Assigns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.ArgumentNullException">state</exception>
        public void Assign(ParserState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Input = state.Input;
            Position = state.Position;
            Nodes = state.Nodes.ToList();
        }

        /// <summary>
        ///     Caches the rule with position and found node.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="pos">The position.</param>
        /// <param name="node">The node.</param>
        public void StoreCache(Rule rule, int pos, Node node)
        {
            if (!_cache.ContainsKey(pos))
                _cache.Add(pos, new Dictionary<Rule, Node>());

            var tmp = _cache[pos];
            if (!tmp.ContainsKey(rule))
                tmp.Add(rule, node);
        }

        /// <summary>
        ///     Tries to get result from cache.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="node">The node.</param>
        /// <returns><c>true</c> if the rule was cached, <c>false</c> otherwise.</returns>
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