﻿using NUnit.Framework;
using ParserLib.Parsing;

namespace ParserLib.Tests
{
    [TestFixture]
    public sealed class SharedGrammarTests
    {
        [Test]
        public void TestDigit()
        {
            var rule = Grammar.Digit;

            Assert.IsTrue(rule.Match("1"));
            Assert.IsFalse(rule.Match("a"));
            Assert.IsFalse(rule.Match(" "));
        }

        [Test]
        public void TestDigits()
        {
            var rule = Grammar.Digits;

            Assert.IsTrue(rule.Match("123"));
            Assert.IsFalse(rule.Match("a1"));
            Assert.IsFalse(rule.Match(" 1"));
        }

        [Test]
        public void TestE()
        {
            var rule = Grammar.E;

            Assert.IsTrue(rule.Match("e+1"));
            Assert.IsTrue(rule.Match("E+1"));
            Assert.IsTrue(rule.Match("e-1"));
        }

        [Test]
        public void TestExponential()
        {
            var rule = Grammar.Exponential;

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
            var rule = Grammar.Float;

            Assert.IsFalse(rule.Match("10"));
            Assert.IsFalse(rule.Match("-10"));
            Assert.IsFalse(rule.Match("+10"));
            Assert.IsTrue(rule.Match("10.23"));
            Assert.IsTrue(rule.Match("-10.23"));
            Assert.IsTrue(rule.Match("+10.23"));
        }

        [Test]
        public void TestHex()
        {
            var rule = Grammar.Hex;

            Assert.IsTrue(rule.Match("A"));
            Assert.IsTrue(rule.Match("b"));
            Assert.IsTrue(rule.Match("1"));
            Assert.IsTrue(rule.Match("0"));
            Assert.IsFalse(rule.Match("g"));
        }

        [Test]
        public void TestHexNumber()
        {
            var rule = Grammar.HexNumber;

            Assert.IsTrue(rule.Match("Ah"));
            Assert.IsTrue(rule.Match("0xb"));
            Assert.IsTrue(rule.Match("0x10AB"));
            Assert.IsTrue(rule.Match("0xDEADBEEF"));
            Assert.IsFalse(rule.Match("g"));
            Assert.IsFalse(rule.Match("DEADBEEF"));
        }

        [Test]
        public void TestInteger()
        {
            var rule = Grammar.Integer;

            Assert.IsTrue(rule.Match("10"));
            Assert.IsTrue(rule.Match("-10"));
            Assert.IsTrue(rule.Match("+10"));
            Assert.IsFalse(rule.Match("10.23"));
            Assert.IsFalse(rule.Match("-10.23"));
            Assert.IsFalse(rule.Match("+10.23"));
        }

        [Test]
        public void TestLabel()
        {
            var rule = Grammar.Label;

            Assert.IsTrue(rule.Match("test"));
            Assert.IsTrue(rule.Match("_test"));
            Assert.IsTrue(rule.Match("_2test"));
            Assert.IsFalse(rule.Match("2abd"));
            Assert.IsFalse(rule.Match("-sdf1"));
        }

        [Test]
        public void TestLetter()
        {
            var rule = Grammar.Letter;

            Assert.IsFalse(rule.Match("1"));
            Assert.IsTrue(rule.Match("a"));
            Assert.IsFalse(rule.Match(" "));
        }

        [Test]
        public void TestLetters()
        {
            var rule = Grammar.Letters;

            Assert.IsFalse(rule.Match("123"));
            Assert.IsTrue(rule.Match("aa"));
            Assert.IsTrue(rule.Match("aa 123"));
            Assert.IsFalse(rule.Match(" 1"));
        }

        [Test]
        public void TestWhitespace()
        {
            var rule = Grammar.Whitespace;

            Assert.IsTrue(rule.Match(" "));
            Assert.IsTrue(rule.Match("\r"));
            Assert.IsFalse(rule.Match("test"));
            Assert.IsFalse(rule.Match("123"));
        }

        [Test]
        public void TestWord()
        {
            var rule = Grammar.Word;

            Assert.IsTrue(rule.Match("word"));
            Assert.IsFalse(rule.Match("\t"));
            Assert.IsTrue(rule.Match("123 regex \\w matches even numbers :O"));
        }
    }
}