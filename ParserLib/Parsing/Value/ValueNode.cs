namespace ParserLib.Parsing.Value
{
    public abstract class ValueNode<T> : Node
    {
        protected ValueNode(string name, string input, int begin) : base(name, input, begin)
        {
        }

        public abstract T Value { get; }
    }
}