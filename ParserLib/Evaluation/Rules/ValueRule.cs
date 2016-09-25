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

        protected abstract ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule);

        protected override Node CreateNode(string name, string input, int begin, Rule matchedRule) => CreateValueNode(name, input, begin, matchedRule);
    }
}