using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing
{
    public sealed class ParserState
    {
        public ParserState(string input, int position)
        {
            Input = input;
            Position = position;
            Childs = new List<Node>();
        }

        public string Input { get; private set; }
        public int Position { get; set; }
        public IList<Node> Childs { get; set; }

        public ParserState Clone() => new ParserState(Input, Position) {Childs = Childs.ToList()};

        public void Assign(ParserState state)
        {
            Input = state.Input;
            Position = state.Position;
            Childs = state.Childs;
        }
    }
}