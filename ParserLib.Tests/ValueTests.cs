using System.Linq;
using NUnit.Framework;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules.Value;
using ParserLib.Parsing.Value;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class ValueTests
    {
        [Test]
        public void TestNestedValue()
        {
            var digits = new ValueFuncRule<int>("digits", int.Parse, SharedGrammar.Digits);
            var letters = new ValueFuncRule<int>("letters", s => s.ToCharArray().Select(a => (int)a).Aggregate((a, b) => a + b), SharedGrammar.Letters);
            var rule = new ValueFuncRule<int>("value", n => n.Leafs.OfType<ValueNode<int>>().Select(v => v.Value).Aggregate((a, b) => a + b), Grammar.OneOrMore(digits | letters));

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