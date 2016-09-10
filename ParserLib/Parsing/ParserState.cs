using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ParserLib.Parsing
{
    [DebuggerStepThrough]
    public sealed class ParserState
    {
        public ParserState(string input)
        {
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
            Input = state.Input;
            Position = state.Position;
            Nodes = state.Nodes;
        }
    }
}