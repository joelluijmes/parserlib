using System;
using System.Linq;
using NUnit.Framework;
using ParserLib.Evaluation;
using ParserLib.Evaluation.Nodes;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class EvaluationTests
    {
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
        public void TestContainsValueNode()
        {
            var node = new Node("", "", 0);
            Assert.IsFalse(Evaluator.ContainsValueNode(node));

            var valueNode = new ConstantValueNode<int>("", "", 0, 0);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(Evaluator.ContainsValueNode(node));
            Assert.IsTrue(Evaluator.ContainsValueNode<int>(node));
            Assert.IsFalse(Evaluator.ContainsValueNode<float>(node));
        }

        [Test]
        public void TestEnumValue()
        {
            var rule = ValueGrammar.MatchEnum<TestEnum, int>("TestEnum");

            Assert.IsTrue(Evaluator.FirstValue<int>(rule.ParseTree("A")) == 1);
        }

        [Test]
        public void TestEvaluateLeafsRule()
        {
            var number = ValueGrammar.ConvertToValue("number", int.Parse, SharedGrammar.Digits);
            var add = ValueGrammar.AccumulateLeafs<int>("add", (left, right) => left + right, number + Grammar.MatchChar('+') + number);
            var subtract = ValueGrammar.AccumulateLeafs<int>("sub", (left, right) => left - right, number + Grammar.MatchChar('-') + number);

            Assert.IsTrue(Evaluator.FirstValue<int>(add.ParseTree("5+6")) == 11);
            Assert.IsTrue(Evaluator.FirstValue<int>(add.ParseTree("15+6")) == 21);
            Assert.IsTrue(Evaluator.FirstValue<int>(subtract.ParseTree("5-6")) == -1);
            Assert.IsTrue(Evaluator.FirstValue<int>(subtract.ParseTree("15-6")) == 9);
        }

        [Test]
        public void TestFirstOrDefaultValue()
        {
            var node = new Node("", "", 0);
            var valueNode = new ConstantValueNode<int>("", "", 0, 100);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(Evaluator.FirstValueOrDefault<int>(node) == 100);
            Assert.IsTrue(Evaluator.FirstValueOrDefault<string>(node) == default(string));
        }

        [Test]
        public void TestFirstValue()
        {
            var node = new Node("", "", 0);
            var valueNode = new ConstantValueNode<int>("", "", 0, 100);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(Evaluator.FirstValue<int>(node) == 100);
            Assert.Throws<EvaluatorException>(() => Evaluator.FirstValue<string>(node));
        }

        [Test]
        public void TestFirstValueNode()
        {
            var node = new Node("", "", 0);
            var valueNode = new ConstantValueNode<int>("", "", 0, 0);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(Evaluator.FirstValueNodeOrDefault(node) != null);
            Assert.IsTrue(Evaluator.FirstValueNodeOrDefault<int>(node) != null);
            Assert.IsTrue(Evaluator.FirstValueNodeOrDefault<float>(node) == null);
        }

        [Test]
        public void TestIsValueNode()
        {
            var node = new Node("", "", 0);
            Assert.IsFalse(Evaluator.IsValueNode(node));

            var valueNode = new ConstantValueNode<int>("", "", 0, 0);
            Assert.IsTrue(Evaluator.IsValueNode(valueNode));
            Assert.IsTrue(Evaluator.IsValueNode<int>(valueNode));
            Assert.IsFalse(Evaluator.IsValueNode<float>(valueNode));
        }

        [Test]
        public void TestNestedValue()
        {
            var digits = ValueGrammar.ConvertToValue("digits", int.Parse, SharedGrammar.Digits);
            Func<string, int> getValueFromLetters = s =>
            {
                var chars = s.ToCharArray(); // convert to seperate chars
                var values = chars.Select(a => (int) a); // convert char to ascii value
                return values.Aggregate((a, b) => a + b); // add the values
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

        [Test]
        public void TestProcess()
        {
            var digits = ValueGrammar.ConvertToValue("digits", int.Parse, SharedGrammar.Digits);
            Func<string, int> getValueFromLetters = s =>
            {
                var chars = s.ToCharArray(); // convert to seperate chars
                var values = chars.Select(a => (int) a); // convert char to ascii value
                return values.Aggregate((a, b) => a + b); // add the values
            };

            var letters = ValueGrammar.ConvertToValue("letters", getValueFromLetters, SharedGrammar.Letters);
            var rule = Grammar.OneOrMore(digits | letters);

            var result = Evaluator.Process<int>(rule.ParseTree("1"), (a, b) => a + b);
            Assert.IsTrue(result == 1);

            result = Evaluator.Process<int>(rule.ParseTree("abcd"), (a, b) => a + b);
            Assert.IsTrue(result == 'a' + 'b' + 'c' + 'd');
        }

        [Test]
        public void TestValueFuncRule()
        {
            var rule = ValueGrammar.ConstantValue("number", 1, SharedGrammar.Digits);

            var node = rule.ParseTree("123");
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 1);
        }
    }
}