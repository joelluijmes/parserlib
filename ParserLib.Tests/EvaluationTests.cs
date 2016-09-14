using System;
using System.Linq;
using NUnit.Framework;
using ParserLib.Evaluation;
using ParserLib.Evaluation.Nodes;
using ParserLib.Evaluation.Rules;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class EvaluationTests
    {
        [Test]
        public void TestValueFuncRule()
        {
            var rule = ValueGrammar.ConstantValue("number", 1, SharedGrammar.Digits);

            var node = rule.ParseTree("123");
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 1);
        }

        [Test]
        public void TestConstantValueRule()
        {
            var rule = ValueGrammar.ConvertToValue("number", int.Parse, SharedGrammar.Digits);

            var node = rule.ParseTree("123");
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 123);
        }

        [Test]
        public void TestNestedValue()
        {
            var digits = ValueGrammar.ConvertToValue("digits", int.Parse, SharedGrammar.Digits);
            Func<string, int> getValueFromLetters = s =>
            {
                var chars = s.ToCharArray();                // convert to seperate chars
                var values = chars.Select(a => (int) a);    // convert char to ascii value
                return values.Aggregate((a, b) => a + b);   // add the values
            };

            var letters = ValueGrammar.ConvertToValue("letters", getValueFromLetters, SharedGrammar.Letters);


            Func<Node, int> getValueFromLeafs = n =>
            {
                var total = 0;
                foreach (var leaf in n.Leafs.OfType<ValueNode<int>>())
                    total += leaf.Value;

                return total;
            };
            var rule = ValueGrammar.ConvertToValue("value", getValueFromLeafs, Grammar.OneOrMore(digits | letters));

            var node = rule.ParseTree("1") as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 1);

            node = rule.ParseTree("a") as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 'a');

            node = rule.ParseTree("1ab") as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 1 + 'a' + 'b');
        }

        
    }
}