﻿using NUnit.Framework;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class SharedGrammarTests
    {
        [Test]
        public void TestAnyCaseInsensitiveString()
        {
            var rule = SharedGrammar.MatchAnyString("dog cat fish", true);

            Assert.IsTrue(rule.Match("dOgfish"));
            Assert.IsTrue(rule.Match("caTfish"));
            Assert.IsTrue(rule.Match("Fishfish"));
            Assert.IsFalse(rule.Match("tacdog"));
        }

        [Test]
        public void TestAnyString()
        {
            var rule = SharedGrammar.MatchAnyString("dog cat fish");

            Assert.IsTrue(rule.Match("dogfish"));
            Assert.IsTrue(rule.Match("catfish"));
            Assert.IsTrue(rule.Match("fishfish"));
            Assert.IsFalse(rule.Match("tacdog"));
        }

        [Test]
        public void TestDigit()
        {
            var rule = SharedGrammar.Digit;

            Assert.IsTrue(rule.Match("1"));
            Assert.IsFalse(rule.Match("a"));
            Assert.IsFalse(rule.Match(" "));
        }

        [Test]
        public void TestDigits()
        {
            var rule = SharedGrammar.Digits;

            Assert.IsTrue(rule.Match("123"));
            Assert.IsFalse(rule.Match("a1"));
            Assert.IsFalse(rule.Match(" 1"));
        }

        [Test]
        public void TestE()
        {
            var rule = SharedGrammar.E;

            Assert.IsTrue(rule.Match("e+1"));
            Assert.IsTrue(rule.Match("E+1"));
            Assert.IsTrue(rule.Match("e-1"));
        }

        [Test]
        public void TestExponential()
        {
            var rule = SharedGrammar.Exponential;

            Assert.IsTrue(rule.Match("e+10"));
            Assert.IsFalse(rule.Match("e^1"));
            Assert.IsFalse(rule.Match("e-"));
            Assert.IsTrue(rule.Match("E+10"));
            Assert.IsTrue(rule.Match("e-10"));
            Assert.IsTrue(rule.Match("e10"));
        }

        [Test]
        public void TestFloat()
        {
            var rule = SharedGrammar.Float;

            Assert.IsFalse(rule.Match("10"));
            Assert.IsFalse(rule.Match("-10"));
            Assert.IsFalse(rule.Match("+10"));
            Assert.IsTrue(rule.Match("10.23"));
            Assert.IsTrue(rule.Match("-10.23"));
            Assert.IsTrue(rule.Match("+10.23"));
        }

        [Test]
        public void TestHexadecimal()
        {
            var rule = SharedGrammar.Hexadecimal;

            Assert.IsTrue(rule.Match("0xAB"));
            Assert.IsTrue(rule.Match("0x1B"));
            Assert.IsTrue(rule.Match("0xa2"));
            Assert.IsTrue(rule.Match("2F"));
            Assert.IsFalse(rule.Match("GE"));
        }

        [Test]
        public void TestInteger()
        {
            var rule = SharedGrammar.Integer;

            Assert.IsTrue(rule.Match("10"));
            Assert.IsTrue(rule.Match("-10"));
            Assert.IsTrue(rule.Match("+10"));
            Assert.IsFalse(rule.Match("10.23"));
            Assert.IsFalse(rule.Match("-10.23"));
            Assert.IsFalse(rule.Match("+10.23"));
        }

        [Test]
        public void TestLetter()
        {
            var rule = SharedGrammar.Letter;

            Assert.IsFalse(rule.Match("1"));
            Assert.IsTrue(rule.Match("a"));
            Assert.IsFalse(rule.Match(" "));
        }

        [Test]
        public void TestLetters()
        {
            var rule = SharedGrammar.Letters;

            Assert.IsFalse(rule.Match("123"));
            Assert.IsTrue(rule.Match("aa"));
            Assert.IsTrue(rule.Match("aa 123"));
            Assert.IsFalse(rule.Match(" 1"));
        }

        [Test]
        public void TestWhitespace()
        {
            var rule = SharedGrammar.Whitespace;

            Assert.IsTrue(rule.Match(" "));
            Assert.IsTrue(rule.Match("\r"));
            Assert.IsFalse(rule.Match("test"));
            Assert.IsFalse(rule.Match("123"));
        }

        [Test]
        public void TestWord()
        {
            var rule = SharedGrammar.Word;

            Assert.IsTrue(rule.Match("word"));
            Assert.IsFalse(rule.Match("\t"));
            Assert.IsTrue(rule.Match("123 regex \\w matches even numbers :O"));
        }
    }
}