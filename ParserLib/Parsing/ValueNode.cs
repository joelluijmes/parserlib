using System;

namespace ParserLib.Parsing
{
    public sealed class ValueNode<T> : Node
    {
        private readonly Func<string, T> _valueFunc;

        public ValueNode(string name, string input, int begin, Func<string, T> valueFunc) : base(name, input, begin)
        {
            _valueFunc = valueFunc;
        }

        public T Value => _valueFunc(Text);
    }
}