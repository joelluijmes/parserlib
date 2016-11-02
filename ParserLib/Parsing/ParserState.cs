using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ParserLib.Parsing
{
    /// <summary>
    ///     Model for holding state of parsing. This class cannot be inherited.
    /// </summary>
    [DebuggerStepThrough]
    public sealed class ParserState
    {
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
            Nodes = Nodes.ToList()
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
    }
}