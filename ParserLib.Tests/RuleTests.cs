﻿using NUnit.Framework;
using ParserLib.Parsing.Rules;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class RuleTests
    {
        [Test]
        public void TestEndRule()
        {
            var rule = new EndRule();

            Assert.IsTrue(rule.Match(""));
            Assert.IsFalse(rule.Match("something"));
        }

        [Test]
        public void TestOneOrMoreRule()
        {
            var rule = new OneOrMoreRule(new StringRule("test "));

            Assert.IsTrue(rule.Match("test something"));
            Assert.IsTrue((rule + new StringRule("something")).Match("test test something"));
            Assert.IsFalse(rule.Match("something"));
        }

        [Test]
        public void TestOptionalRule()
        {
            var rule = new OptionalRule(new StringRule("test"));

            Assert.IsTrue(rule.Match("test"));
            Assert.IsTrue(rule.Match("something"));
        }

        [Test]
        public void TestOrRule()
        {
            var rule = new OrRule(new StringRule("cat"), new StringRule("dog"));

            Assert.IsFalse(rule.Match("something"));
            Assert.IsFalse(rule.Match("fishcatdog"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("dogfish"));
        }

        [Test]
        public void TestOrUsingOperatorRule()
        {
            var rule = new StringRule("cat") | new StringRule("dog");

            Assert.IsFalse(rule.Match("something"));
            Assert.IsFalse(rule.Match("fishcatdog"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("dogfish"));
        }

        [Test]
        public void TestRegexRule()
        {
            var wordRule = new RegexRule("\\d");

            Assert.IsTrue(wordRule.Match("1"));
            Assert.IsTrue(wordRule.Match("123"));
            Assert.IsFalse(wordRule.Match("test"));
        }

        [Test]
        public void TestSequenceRule()
        {
            var rule = new SequenceRule(new StringRule("cat"), new StringRule("fish"));

            Assert.IsFalse(rule.Match("catsomething"));
            Assert.IsFalse(rule.Match("fishcat"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("catfish something"));
        }

        [Test]
        public void TestSequenceUsingOperatorRule()
        {
            var rule = new StringRule("cat") + new StringRule("fish");

            Assert.IsFalse(rule.Match("catsomething"));
            Assert.IsFalse(rule.Match("fishcat"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("catfish something"));
        }

        [Test]
        public void TestStartRule()
        {
            var rule = new StartRule();
            var randomRule = new StringRule("Test");

            Assert.IsTrue(rule.Match("sometihng"));
            Assert.IsFalse((randomRule + rule).Match("Test dingen"));
        }

        [Test]
        public void TestStringRule()
        {
            var rule = new StringRule("Test");

            Assert.IsTrue(rule.Match("Test"));
            Assert.IsTrue(rule.Match("Test123"));
            Assert.IsFalse(rule.Match("Failing Test"));
        }

        [Test]
        public void TestZeroOrMoreRule()
        {
            var rule = new ZeroOrMoreRule(new StringRule("test"));

            Assert.IsTrue(rule.Match("something"));
            Assert.IsTrue(rule.Match("test test something"));
        }
    }
}