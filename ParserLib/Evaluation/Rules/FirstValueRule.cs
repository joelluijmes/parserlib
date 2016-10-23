using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class FirstValueRule<T> : ValueRule<T>
    {
        public FirstValueRule(Rule rule) : base(rule)
        {
        }

        public FirstValueRule(string name, Rule rule) : base(name, rule)
        {
        }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin, Rule matchedRule) => new LazyValueNode<T>(name, input, begin, GetValue, matchedRule);

        private T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            var valueLeafs = valueNode.ChildLeafs.SelectMany(leaf => leaf.WhereLeafs(NodeExtensions.IsValueNode<T>)).Cast<ValueNode<T>>();
            return valueLeafs.First().Value;
        }
    }
}