using System;
using ParserLib.Parsing.Value;

namespace ParserLib.Parsing.Rules.Value
{
    public sealed class ValueFuncRule<T> : NodeRule
    {
        public ValueFuncRule(string name, Func<T> valueFunc, Rule rule) : base(name, rule)
        {
            ValueFunc = node => valueFunc();
        }

        public ValueFuncRule(string name, Func<string, T> valueFunc, Rule rule) : base(name, rule)
        {
            ValueFunc = node => valueFunc(node.Text);
        }

        public ValueFuncRule(string name, Func<ValueNode<T>, T> valueFunc, Rule rule) : base(name, rule)
        {
            ValueFunc = valueFunc;
        }

        public Func<ValueNode<T>, T> ValueFunc { get; }

        protected override Node CreateNode(string name, string input, int begin) => new ValueFuncNode<T>(name, input, begin, ValueFunc);
    }
}