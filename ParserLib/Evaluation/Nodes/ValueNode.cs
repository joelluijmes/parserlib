using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    public abstract class ValueNode<T> : Node
    {
        protected ValueNode(string name, string input, int begin, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
        }

        public abstract T Value { get; }
    }
}