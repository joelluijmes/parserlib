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
            var rule = Evaluator.ConstantValue("number", 1, SharedGrammar.Digits);

            var node = rule.ParseTree("123").First();
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 1);
        }

        [Test]
        public void TestConstantValueRule()
        {
            var rule = Evaluator.ConvertToValue("number", int.Parse, SharedGrammar.Digits);

            var node = rule.ParseTree("123").First();
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 123);
        }

        [Test]
        public void TestNestedValue()
        {
            var digits = new ConvertToValueRule<int>("digits", int.Parse, SharedGrammar.Digits);
            Func<string, int> getValueFromLetters = s =>
            {
                var chars = s.ToCharArray();                // convert to seperate chars
                var values = chars.Select(a => (int) a);    // convert char to ascii value
                return values.Aggregate((a, b) => a + b);   // add the values
            };

            var letters = new ConvertToValueRule<int>("letters", getValueFromLetters, SharedGrammar.Letters);
            var rule = new ConvertToValueRule<int>("value", n => n.Leafs.OfType<ValueNode<int>>().Select(v => v.Value).Aggregate((a, b) => a + b), Grammar.OneOrMore(digits | letters));

            var node = rule.ParseTree("1").First() as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 1);

            node = rule.ParseTree("a").First() as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 'a');

            node = rule.ParseTree("1ab").First() as ValueNode<int>;
            Assert.IsTrue(node != null);
            Assert.IsTrue(node.Value == 1 + 'a' + 'b');
        }

        
    }
}