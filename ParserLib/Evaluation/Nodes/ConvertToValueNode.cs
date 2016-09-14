using System;

namespace ParserLib.Evaluation.Nodes
{
    public class ConvertToValueNode<T> : ValueNode<T>
    {
        private readonly Func<ValueNode<T>, T> _valueFunc;

        public ConvertToValueNode(string name, string input, int begin, Func<ValueNode<T>, T> valueFunc) : base(name, input, begin)
        {
            _valueFunc = valueFunc;
        }

        public override T Value => _valueFunc(this);
    }
}