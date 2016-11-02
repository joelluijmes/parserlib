using System.Collections.Generic;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Nodes a rule. It is used for creating a tree structure of rules and nodes. All node rule uses intermediate caching.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public class NodeRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NodeRule" /> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public NodeRule(Rule rule) : base(rule)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NodeRule" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rule">The rule.</param>
        public NodeRule(string name, Rule rule) : base(rule)
        {
            Name = name;
        }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => FirstChild.Definition;

        /// <summary>
        ///     Specific rule implementation of the match. Which caches the result.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state)
        {
            var node = CreateNode(Name, state.Input, state.Position, this);
            var oldChilds = state.Nodes;
            state.Nodes = new List<Node>();

            if (!FirstChild.MatchImpl(state))
            {
                state.Nodes = oldChilds;
                return false;
            }

            node.End = state.Position;
            node.ChildLeafs = state.Nodes;

            oldChilds.Add(node);
            state.Nodes = oldChilds;
            return true;
        }

        /// <summary>
        ///     Creates the specific node. Can be overriden.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="matchedRule">The matched rule.</param>
        /// <returns>Node.</returns>
        protected virtual Node CreateNode(string name, string input, int begin, Rule matchedRule) => new Node(name, input, begin, matchedRule);
    }
}