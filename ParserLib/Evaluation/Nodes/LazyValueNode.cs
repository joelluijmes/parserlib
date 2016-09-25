using System;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Nodes
{
    public class LazyValueNode<T> : ValueNode<T>
    {
        private readonly Func<ValueNode<T>, T> _valueFunc;
        private T _value;
        private bool _valueSet;

        public LazyValueNode(string name, string input, int begin, Func<ValueNode<T>, T> valueFunc, Rule matchedRule) : base(name, input, begin, matchedRule)
        {
            _valueFunc = valueFunc;
        }

        public override T Value
        {
            get
            {
                if (_valueSet)
                    return _value;

                _valueSet = true;
                return _value = _valueFunc(this);
            }
        }
    }
}