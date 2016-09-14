using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public abstract class ValueRule<T> : NodeRule
    {
        protected ValueRule(string name, Rule rule) : base(name, rule)
        {
        }

        protected abstract ValueNode<T> CreateValueNode(string name, string input, int begin);

        protected override Node CreateNode(string name, string input, int begin) => CreateValueNode(name, input, begin);
    }
}