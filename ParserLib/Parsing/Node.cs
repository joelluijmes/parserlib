using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Model where results of parsing are stored.
    /// </summary>
    public class Node
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="matchedRule">The matched rule.</param>
        public Node(string input, Rule matchedRule) : this(null, input, input.Length, matchedRule)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="matchedRule">The matched rule.</param>
        public Node(string name, string input, Rule matchedRule) : this(name, input, input.Length, matchedRule)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <param name="begin">The first position to start parsing.</param>
        /// <param name="matchedRule">The matched rule.</param>
        /// <exception cref="System.ArgumentNullException">input</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">begin - Begin cannot be negative nor higher than the input length</exception>
        public Node(string name, string input, int begin, Rule matchedRule)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if ((begin < 0) || (begin > input.Length))
                throw new ArgumentOutOfRangeException(nameof(begin), "Begin cannot be negative nor higher than the input length");

            Name = name;
            Input = input;
            Begin = begin;
            MatchedRule = matchedRule;
            End = input.Length;
            ChildLeafs = new List<Node>();
        }

        /// <summary>
        ///     Gets or sets the begin.
        /// </summary>
        /// <value>The begin.</value>
        internal int Begin { get; set; }

        /// <summary>
        ///     Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        internal int End { get; set; }

        /// <summary>
        ///     Gets or sets the input to be parsed.
        /// </summary>
        /// <value>The input.</value>
        internal string Input { get; set; }

        /// <summary>
        ///     Gets the length left to parse.
        /// </summary>
        /// <value>The length.</value>
        internal int Length => End > Begin ? End - Begin : 0;

        /// <summary>
        ///     Gets or sets the child leafs.
        /// </summary>
        /// <value>The child leafs.</value>
        internal IList<Node> ChildLeafs { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets the leafs in a readonly collection.
        /// </summary>
        /// <value>The leafs.</value>
        public IList<Node> Leafs => new ReadOnlyCollection<Node>(ChildLeafs);

        /// <summary>
        ///     Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text => Input.Substring(Begin, Length);

        /// <summary>
        ///     Gets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value><c>true</c> if this instance is leaf; otherwise, <c>false</c>.</value>
        public bool IsLeaf => !ChildLeafs.Any();

        /// <summary>
        ///     Gets the matched rule.
        /// </summary>
        /// <value>The matched rule.</value>
        public Rule MatchedRule { get; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"{Name ?? "anon"}: {Text}";
    }
}