using System;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Evaluation.Rules
{
    public sealed class ConvertToValueRule<T> : ValueRule<T>
    {
        public ConvertToValueRule(string name, Func<Node, T> converter, Rule rule) : base(name, rule)
        {
            Converter = converter;
        }

        public ConvertToValueRule(string name, Func<string, T> converter, Rule rule) : base(name, rule)
        {
            Converter = node => converter(node.Text);
        }

        public Func<Node, T> Converter { get; }

        protected override ValueNode<T> CreateValueNode(string name, string input, int begin) => new ConvertToValueNode<T>(name, input, begin, Converter);
    }
}