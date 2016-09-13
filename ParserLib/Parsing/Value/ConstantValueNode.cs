namespace ParserLib.Parsing.Value
{
    public class ConstantValueNode<T> : ValueNode<T>
    {
        public ConstantValueNode(string name, string input, int begin, T value) : base(name, input, begin)
        {
            Value = value;
        }

        public override T Value { get; }
    }
}