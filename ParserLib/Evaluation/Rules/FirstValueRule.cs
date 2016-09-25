using System.Linq;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class FirstValueRule<T> : ValueRule<T>
    {
        public FirstValueRule(string name, Rule rule) : base(name, rule)
        {
        }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin) => new LazyValueNode<T>(name, input, begin, GetValue);

        private T GetValue(ValueNode<T> valueNode)
        {
            // Get all ValueNode<T> in the leafs :)
            var valueLeafs = valueNode.ChildLeafs.SelectMany(leaf => leaf.WhereLeafs(NodeExtensions.IsValueNode<T>)).Cast<ValueNode<T>>();
            return valueLeafs.First().Value;
        }
    }
}