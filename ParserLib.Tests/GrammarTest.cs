using System;
using NUnit.Framework;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib.Tests
{
	[TestFixture]
	public sealed class GrammarTests
	{
		[Test]
		public void TestAnyCaseInsensitiveString()
		{
			var rule = Grammar.MatchAnyString("dog cat fish", true);

			Assert.IsTrue(rule.Match("dOgfish"));
			Assert.IsTrue(rule.Match("caTfish"));
			Assert.IsTrue(rule.Match("Fishfish"));
			Assert.IsFalse(rule.Match("tacdog"));
		}

		[Test]
		public void TestAnyChar()
		{
			var rule = Grammar.MatchAnyChar('a', 'b', 'c', 'd');

			Assert.IsTrue(rule.Match("a"));
			Assert.IsTrue(rule.Match("b"));
			Assert.IsTrue(rule.Match("d"));
			Assert.IsFalse(rule.Match("q"));

			rule = Grammar.MatchAnyChar();
			Assert.IsTrue(rule.Match("a"));
			Assert.IsTrue(rule.Match("x"));
		}

		[Test]
		public void TestAnyString()
		{
			var rule = Grammar.MatchAnyString("dog cat fish");

			Assert.IsTrue(rule.Match("dogfish"));
			Assert.IsTrue(rule.Match("catfish"));
			Assert.IsTrue(rule.Match("fishfish"));
			Assert.IsFalse(rule.Match("tacdog"));
		}

		[Test]
		public void TestBinaryRule()
		{
			var left = Grammar.Node("letter", Grammar.Char(char.IsLetter));
			var right = Grammar.Node("digit", Grammar.Char(char.IsDigit));
			var add = Grammar.Node("add", Grammar.MatchAnyString("+ add"));
			var sub = Grammar.Node("sub", Grammar.MatchAnyString("- sub"));
			var and = Grammar.Node("and", Grammar.MatchAnyString("& and"));
			var or = Grammar.Node("or", Grammar.MatchAnyString("| or"));
			var op = add | sub | and | or;

			var ruleFixed = Grammar.Binary(left, op, right, false);
			Assert.IsTrue(ruleFixed.Match("k+2"));
			Assert.IsFalse(ruleFixed.Match("2+k"));

			var ruleUnfixed = Grammar.Binary(left, op, right);
			Assert.IsTrue(ruleUnfixed.Match("k+2"));
			Assert.IsTrue(ruleUnfixed.Match("2+k"));
		}

		[Test]
		public void TestChar()
		{
			var rule = Grammar.Char(char.IsDigit);

			Assert.IsTrue(rule.Match("1"));
			Assert.IsFalse(rule.Match("a"));
		}

		[Test]
		public void TestEndRule()
		{
			var rule = Grammar.MatchString("str") + Grammar.End();

			Assert.IsTrue(rule.Match("str"));
			Assert.IsFalse(rule.Match("something"));
		}

		[Test]
		public void TestMatchChar()
		{
			var rule = Grammar.MatchChar('a');

			Assert.IsTrue(rule.Match("a"));
			Assert.IsFalse(rule.Match("A"));
			Assert.IsFalse(rule.Match("q"));
		}

		[Test]
		public void TestMatchCharCaseInsensitive()
		{
			var rule = Grammar.MatchChar('a', true);

			Assert.IsTrue(rule.Match("a"));
			Assert.IsTrue(rule.Match("A"));
			Assert.IsFalse(rule.Match("q"));
		}

		[Test]
		public void TestNode()
		{
			var rule = Grammar.Node(Grammar.Digit);

			Assert.IsTrue(rule.Match("1"));
			Assert.IsTrue(rule.Match("2"));
			Assert.IsFalse(rule.Match("x"));
			Assert.Throws<ArgumentNullException>(() => Grammar.Node(null));
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
		public void TestRecursive()
		{
			var op = Grammar.MatchAnyString("+ -");
			var digit = new RegexRule("\\d+");

			Rule expressionA = null;
			var recursiveExpression = Grammar.Func(() => expressionA);

			var expression = digit + recursiveExpression;
			expressionA = (op + digit + recursiveExpression) | Grammar.End();

			Assert.IsTrue(expression.Match("1"));
			Assert.IsTrue(expression.Match("1+2"));
			Assert.IsTrue(expression.Match("1+2+3"));
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
		public void TestStringCaseInsensitiveRule()
		{
			var rule = Grammar.MatchString("Test", true);

			Assert.IsTrue(rule.Match("Test"));
			Assert.IsTrue(rule.Match("Test123"));
			Assert.IsTrue(rule.Match("test123"));
			Assert.IsFalse(rule.Match("Failing Test"));
		}

		[Test]
		public void TestStringRule()
		{
			var rule = Grammar.MatchString("Test");

			Assert.IsTrue(rule.Match("Test"));
			Assert.IsTrue(rule.Match("Test123"));
			Assert.IsFalse(rule.Match("test123"));
			Assert.IsFalse(rule.Match("Failing Test"));
		}

		[Test]
		public void TestWhile()
		{
			var rule = Grammar.MatchWhile(Grammar.Char(char.IsDigit));

			Assert.IsTrue(rule.Match("123"));
			Assert.IsTrue(rule.Match("123Test"));
			Assert.IsFalse(rule.Match("test123"));
		}

		[Test]
		public void TestZeroOrMoreRule()
		{
			var rule = Grammar.ZeroOrMore(Grammar.MatchString("test"));

			Assert.IsTrue(rule.Match("something"));
			Assert.IsTrue(rule.Match("test test something"));
		}
	}
}