using NUnit.Framework;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class DefinitionTests
    {
        [Test]
        public void TestChar()
        {
            Assert.AreEqual("'a'", Grammar.MatchChar('a').Definition);
            Assert.AreEqual("f(char)", Grammar.Char(f => f == 'a').Definition);
            Assert.AreEqual("IsDigit", Grammar.Char(char.IsDigit).Definition);
        }

        [Test]
        public void TestEnd()
        {
            Assert.AreEqual("$", Grammar.End().Definition);
        }

        [Test]
        public void TestStart()
        {
            Assert.AreEqual("^", Grammar.Start().Definition);
        }

        [Test]
        public void TestFunc()
        {   // quite useless due FirstChild.Definition
            Assert.AreEqual("$", Grammar.Func(Grammar.End).Definition);
        }

        [Test]
        public void TestNode()
        {   // quite useless due FirstChild.Definition
            Assert.AreEqual("$", Grammar.Func(Grammar.End).Definition);
        }

        [Test]
        public void TestNot()
        {
            Assert.AreEqual("Not('a')", Grammar.Not(Grammar.MatchChar('a')).Definition);
        }

        [Test]
        public void TestOneOrMore()
        {
            Assert.AreEqual("('a')+", Grammar.OneOrMore(Grammar.MatchChar('a')).Definition);
        }

        [Test]
        public void TestZeroOrMore()
        {
            Assert.AreEqual("('a')*", Grammar.ZeroOrMore(Grammar.MatchChar('a')).Definition);
        }

        [Test]
        public void TestOptional()
        {
            Assert.AreEqual("('a')?", Grammar.Optional(Grammar.MatchChar('a')).Definition);
        }

        [Test]
        public void TestRegex()
        {
            Assert.AreEqual("regex([\\s])", Grammar.Regex("[\\s]").Definition);
        }

        [Test]
        public void TestString()
        {
            Assert.AreEqual("\"cat\"", Grammar.MatchString("cat").Definition);
        }

        [Test]
        public void TestOr()
        {
            Assert.AreEqual("('a' | 'b')", (Grammar.MatchChar('a') | Grammar.MatchChar('b')).Definition);
            Assert.AreEqual("('a' | 'b' | 'c')", (Grammar.MatchChar('a') | Grammar.MatchChar('b') | Grammar.MatchChar('c')).Definition);
            Assert.AreEqual("('a' | 'b' | 'c' | 'd')", (Grammar.MatchChar('a') | (Grammar.MatchChar('b') | Grammar.MatchChar('c')) | Grammar.MatchChar('d')).Definition);
        }

        [Test]
        public void TestSequence()
        {
            Assert.AreEqual("('a' + 'b')", (Grammar.MatchChar('a') + Grammar.MatchChar('b')).Definition);
            Assert.AreEqual("('a' + 'b' + 'c')", (Grammar.MatchChar('a') + Grammar.MatchChar('b') + Grammar.MatchChar('c')).Definition);
            Assert.AreEqual("('a' + 'b' + 'c' + 'd')", (Grammar.MatchChar('a') + (Grammar.MatchChar('b') + Grammar.MatchChar('c')) + Grammar.MatchChar('d')).Definition);
        }
    }
}