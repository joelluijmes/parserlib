using NUnit.Framework;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class StringTests
    {
        [Test]
        public void TestNode()
        {
            var rule = Grammar.Node(Grammar.MatchChar('a'));
            Assert.AreEqual("anon: a", rule.ParseTree("a").ToString());

            rule = Grammar.Node("node", Grammar.MatchChar('a'));
            Assert.AreEqual("node: a", rule.ParseTree("a").ToString());
        }
    }
}