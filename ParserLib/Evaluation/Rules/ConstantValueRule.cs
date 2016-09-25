using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class ConstantValueRule<T> : ValueRule<T>
    {
        public ConstantValueRule(string name, T value, Rule rule) : base(name, rule)
        {
            Value = value;
        }

        public T Value { get; }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new ConstantValueNode<T>(name, input, begin, Value, matchedRule);
    }
}