using ParserLib.Parsing.Value;

namespace ParserLib.Parsing.Rules.Value
{
    public sealed class ConstantValueRule<T> : NodeRule
    {
        public ConstantValueRule(string name, T value, Rule rule) : base(name, rule)
        {
            Value = value;
        }

        public T Value { get; }

        protected override Node CreateNode(string name, string input, int begin) => new ConstantValueNode<T>(name, input, begin, Value);
    }
}