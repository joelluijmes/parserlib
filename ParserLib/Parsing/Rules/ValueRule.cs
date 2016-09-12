using System;

namespace ParserLib.Parsing.Rules
{
    public sealed class ValueRule<T> : NodeRule
    {
        private readonly Func<string, T> _valueFunc;

        public ValueRule(string name, Func<string, T> valueFunc, Rule rule) : base(name, rule)
        {
            _valueFunc = valueFunc;
        }

        protected override Node CreateNode(string name, string input, int begin) => new ValueNode<T>(name, input, begin, _valueFunc);
    }
}