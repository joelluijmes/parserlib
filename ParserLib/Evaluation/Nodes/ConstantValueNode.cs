using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    public class ConstantValueNode<T> : ValueNode<T>
    {
        public ConstantValueNode(string name, string input, int begin, T value, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
            Value = value;
        }

        public override T Value { get; }
    }
}