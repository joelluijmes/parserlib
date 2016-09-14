using ParserLib.Parsing;

namespace ParserLib.Evaluation.Nodes
{
    public abstract class ValueNode<T> : Node
    {
        protected ValueNode(string name, string input, int begin) : base(name, input, begin)
        {
        }

        public abstract T Value { get; }
    }
}