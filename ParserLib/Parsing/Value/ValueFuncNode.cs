using System;

namespace ParserLib.Parsing.Value
{
    public class ValueFuncNode<T> : ValueNode<T>
    {
        private readonly Func<ValueNode<T>, T> _valueFunc;

        public ValueFuncNode(string name, string input, int begin, Func<ValueNode<T>, T> valueFunc) : base(name, input, begin)
        {
            _valueFunc = valueFunc;
        }

        public override T Value => _valueFunc(this);
    }
}