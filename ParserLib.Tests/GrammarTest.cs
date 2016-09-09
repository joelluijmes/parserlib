using NUnit.Framework;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class GrammarTests
    {
        [Test]
        public void TestEndRule()
        {
            var rule = Grammar.End();

            Assert.IsTrue(rule.Match(""));
            Assert.IsFalse(rule.Match("something"));
        }

        [Test]
        public void TestOneOrMoreRule()
        {
            var rule = Grammar.OneOrMore(Grammar.MatchString("test "));

            Assert.IsTrue(rule.Match("test something"));
            Assert.IsTrue((rule + Grammar.MatchString("something")).Match("test test something"));
            Assert.IsFalse(rule.Match("something"));
        }

        [Test]
        public void TestOptionalRule()
        {
            var rule = Grammar.Optional(Grammar.MatchString("test"));

            Assert.IsTrue(rule.Match("test"));
            Assert.IsTrue(rule.Match("something"));
        }

        [Test]
        public void TestOrRule()
        {
            var rule = Grammar.Or(Grammar.MatchString("cat"), Grammar.MatchString("dog"));

            Assert.IsFalse(rule.Match("something"));
            Assert.IsFalse(rule.Match("fishcatdog"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("dogfish"));
        }

        [Test]
        public void TestRegexRule()
        {
            var rule = Grammar.Regex("\\d");

            Assert.IsTrue(rule.Match("1"));
            Assert.IsTrue(rule.Match("123"));
            Assert.IsFalse(rule.Match("test"));
        }

        [Test]
        public void TestSequenceRule()
        {
            var rule = Grammar.Sequence(Grammar.MatchString("cat"), Grammar.MatchString("fish"));

            Assert.IsFalse(rule.Match("catsomething"));
            Assert.IsFalse(rule.Match("fishcat"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("catfish something"));
        }

        [Test]
        public void TestStartRule()
        {
            var rule = Grammar.Start();
            var randomRule = Grammar.MatchString("Test");

            Assert.IsTrue(rule.Match("sometihng"));
            Assert.IsFalse((randomRule + rule).Match("Test dingen"));
        }

        [Test]
        public void TestStringRule()
        {
            var rule = Grammar.MatchString("Test");

            Assert.IsTrue(rule.Match("Test"));
            Assert.IsTrue(rule.Match("Test123"));
            Assert.IsFalse(rule.Match("Failing Test"));
        }

        [Test]
        public void TestZeroOrMoreRule()
        {
            var rule = Grammar.ZeroOrMore(Grammar.MatchString("test"));

            Assert.IsTrue(rule.Match("something"));
            Assert.IsTrue(rule.Match("test test something"));
        }

        [Test]
        public void TestChar()
        {
            var rule = Grammar.Char(char.IsDigit);

            Assert.IsTrue(rule.Match("1"));
            Assert.IsFalse(rule.Match("a"));
        }
    }
}