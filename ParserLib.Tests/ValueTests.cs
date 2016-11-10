using System;
using System.Linq;
using NUnit.Framework;
using ParserLib.Evaluation;
using ParserLib.Evaluation.Nodes;
using ParserLib.Exceptions;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class ValueTests
    {
        [Test]
        public void TestAccumulate()
        {
            var number = Grammar.ConvertToValue("number", int.Parse, Grammar.Digits);
            var add = Grammar.Accumulate<int>("add", (left, right) => left + right, number + Grammar.MatchChar('+') + number);
            var subtract = Grammar.Accumulate<int>("sub", (left, right) => left - right, number + Grammar.MatchChar('-') + number);

            Assert.AreEqual(11, add.ParseTree("5+6").FirstValue<int>());
            Assert.AreEqual(21, add.ParseTree("15+6").FirstValue<int>());
            Assert.AreEqual(-1, subtract.ParseTree("5-6").FirstValue<int>());
            Assert.AreEqual(9, subtract.ParseTree("15-6").FirstValue<int>());

            var convertRule = Grammar.ConvertToValue("convert", int.Parse, Grammar.Digit);
            var rule = 
                Grammar.Node("node",
                    Grammar.Accumulate<int>("accumulate", (a, b) => a + b,
                        Grammar.FirstValue<int>("fv_conv",
                            convertRule)));
            
            Assert.AreEqual(2, rule.FirstValue<int>("2"));

            rule =
                Grammar.Accumulate<int>("accumulate", (a, b) => a + b,
                    convertRule + Grammar.MatchChar('+') + convertRule);
            Assert.AreEqual(4, rule.FirstValue<int>("2+2"));
        }

        [Test]
        public void TestBinary()
        {
            var rule = Grammar.Binary(3);
            Assert.AreEqual("001", rule.FirstValue("1"));
            Assert.AreEqual("101", rule.FirstValue("5"));
            Assert.AreEqual("000", rule.FirstValue("8"));

            var number = Grammar.ConvertToValue("number", int.Parse, Grammar.Digits);
            var add = Grammar.Accumulate<int>("add", (left, right) => left + right, number + Grammar.MatchChar('+') + number);
            rule = Grammar.ConvertBinary(add);
            Assert.AreEqual("10", rule.FirstValue("1+1"));
            Assert.AreEqual("100", rule.FirstValue("3+1"));

            rule = Grammar.ConvertBinary(add, 4);
            Assert.AreEqual("0010", rule.FirstValue("1+1"));
            Assert.AreEqual("0100", rule.FirstValue("3+1"));
            Assert.AreEqual("0001", rule.FirstValue("10+7"));

            rule = Grammar.Binary(4);
            Assert.AreEqual("1111", rule.FirstValue("-1"));
            Assert.AreEqual("1110", rule.FirstValue("-2"));
            Assert.AreEqual("1101", rule.FirstValue("-3"));
            Assert.AreEqual("0000", rule.FirstValue("-16"));
        }

        [Test]
        public void TestConstantValueRule()
        {
            var rule = Grammar.ConvertToValue("number", int.Parse, Grammar.Digits);

            var node = rule.ParseTree("123");
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 123);
        }

        [Test]
        public void TestContainsValueNode()
        {
            var node = new Node("", "", null);
            Assert.IsFalse(node.ContainsValueNode());

            var valueNode = new ConstantValueNode<int>("", "", 0, 0, null);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(node.ContainsValueNode());
            Assert.IsTrue(node.ContainsValueNode<int>());
            Assert.IsFalse(node.ContainsValueNode<float>());
        }

        [Test]
        public void TestEnum()
        {
            var rule = Grammar.EnumValue<TestEnum, int>(Tests.TestEnum.A);
            Assert.IsTrue(rule.ParseTree(Tests.TestEnum.A.ToString()).FirstValue<int>() == (int) Tests.TestEnum.A);
        }

        [Test]
        public void TestFirstNodeByName()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);
            var parsed = rule.ParseTree("123");

            Assert.IsTrue(parsed.FirstNodeByName("number") != null);
            Assert.Throws<EvaluatorException>(() => parsed.FirstNodeByName("something"));
        }

        [Test]
        public void TestFirstNodeOrDefaultByName()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);
            var parsed = rule.ParseTree("123");

            Assert.IsTrue(parsed.FirstNodeByNameOrDefault("number") != null);
            Assert.IsTrue(parsed.FirstNodeByNameOrDefault("something") == null);
        }

        [Test]
        public void TestFirstOrDefaultValue()
        {
            var node = new Node("", "", null);
            var valueNode = new ConstantValueNode<int>("", "", 0, 100, null);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(node.FirstValueOrDefault<int>() == 100);
            Assert.IsTrue(node.FirstValueOrDefault<string>() == default(string));
        }

        [Test]
        public void TestFirstValue()
        {
            var node = new Node("", "", null);
            var valueNode = new ConstantValueNode<int>("", "", 0, 100, null);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(node.FirstValue<int>() == 100);
            Assert.Throws<EvaluatorException>(() => node.FirstValue<string>());
        }

        [Test]
        public void TestFirstValueByName()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);
            var parsed = rule.ParseTree("123");

            Assert.AreEqual(1, parsed.FirstValueByName<int>("number"));
            Assert.Throws<EvaluatorException>(() => parsed.FirstValueByName<int>("something"));
            Assert.Throws<EvaluatorException>(() => parsed.FirstValueByName<float>("number"));
        }

        [Test]
        public void TestFirstValueNode()
        {
            var node = new Node("", "", null);
            var valueNode = new ConstantValueNode<int>("", "", 0, 0, null);
            node.Leafs.Add(valueNode);

            Assert.IsTrue(node.FirstValueNodeOrDefault<int>() != null);
            Assert.IsTrue(node.FirstValueNodeOrDefault<float>() == null);
        }

        [Test]
        public void TestFirstValueOrDefaultByName()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);
            var parsed = rule.ParseTree("123");

            Assert.AreEqual(1, parsed.FirstValueByNameOrDefault<int>("number"));
            Assert.IsTrue(parsed.FirstValueByNameOrDefault<int>("something") == default(int));
            Assert.IsTrue(parsed.FirstValueByNameOrDefault<float?>("number") == default(float?));
        }

        [Test]
        public void TestIsValueNode()
        {
            var node = new Node("", "", null);
            Assert.IsFalse(node.IsValueNode());

            var valueNode = new ConstantValueNode<int>("", "", 0, 0, null);
            Assert.IsTrue(valueNode.IsValueNode());
            Assert.IsTrue(valueNode.IsValueNode<int>());
            Assert.IsFalse(valueNode.IsValueNode<float>());
        }

        [Test]
        public void TestMatchEnum()
        {
            var rule = Grammar.EnumValue<TestEnum, int>("TestEnum");
            Assert.IsTrue(rule.ParseTree(Tests.TestEnum.A.ToString()).FirstValue<int>() == (int) Tests.TestEnum.A);

            var ruleb = Grammar.EnumValue<TestEnum, TestEnum>("TestEnum");
            Assert.IsTrue(ruleb.ParseTree(Tests.TestEnum.B.ToString()).FirstValue<TestEnum>() == Tests.TestEnum.B);
        }

        [Test]
        public void TestNumber()
        {
            var rule = Grammar.Int32("immediate");

            Assert.AreEqual(10, rule.ParseTree("10").FirstValue<int>());
            Assert.AreEqual(0x10, rule.ParseTree("10h").FirstValue<int>());
            Assert.AreEqual(16, rule.ParseTree("0x10").FirstValue<int>());
            Assert.AreEqual(10, rule.ParseTree("0x0A").FirstValue<int>());
            Assert.AreEqual(0xABC, rule.ParseTree("0xABC").FirstValue<int>());
            Assert.IsFalse(rule.Match("abc"));
        }

        [Test]
        public void TestProcess()
        {
            var digits = Grammar.ConvertToValue("digits", int.Parse, Grammar.Digits);
            Func<string, int> getValueFromLetters = s =>
            {
                var chars = s.ToCharArray(); // convert to seperate chars
                var values = chars.Select(a => (int) a); // convert char to ascii value
                return values.Aggregate((a, b) => a + b); // add the values
            };

            var letters = Grammar.ConvertToValue("letters", getValueFromLetters, Grammar.Letters);
            var rule = Grammar.OneOrMore(digits | letters);

            var result = rule.ParseTree("1").Process<int>((a, b) => a + b);
            Assert.IsTrue(result == 1);

            result = rule.ParseTree("abcd").Process<int>((a, b) => a + b);
            Assert.IsTrue(result == 'a' + 'b' + 'c' + 'd');
        }

        [Test]
        public void TestRangeRule()
        {
            var rule = Grammar.Range(0, 10, Grammar.Int32());

            Assert.AreEqual(0, rule.FirstValue("0"));
            Assert.AreEqual(5, rule.FirstValue("5"));
            Assert.AreEqual(10, rule.FirstValue("10"));
            Assert.Throws<EvaluatorException>(() => rule.FirstValue("11"));
        }

        [Test]
        public void TestTryGetValue()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);
            var parsed = rule.ParseTree("123");

            int output;
            Assert.IsTrue(parsed.TryGetValue(out output));
            Assert.AreEqual(1, output);

            parsed = rule.Optional.ParseTree("");
            Assert.IsFalse(parsed.TryGetValue(out output));
            Assert.AreEqual(default(int), output);
        }

        [Test]
        public void TestValueFuncRule()
        {
            var rule = Grammar.ConstantValue("number", 1, Grammar.Digits);

            var node = rule.ParseTree("123");
            var valueNode = node as ValueNode<int>;
            Assert.IsTrue(valueNode != null);
            Assert.IsTrue(valueNode.Value == 1);
        }
    }
}