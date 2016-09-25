using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public class Node
    {
        public Node(string name, string input, Rule matchedRule) : this(name, input, input.Length, matchedRule)
        { 
        }

        public Node(string name, string input, int begin, Rule matchedRule)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if ((begin < 0) || (begin > input.Length))
                throw new ArgumentOutOfRangeException(nameof(begin), "Begin cannot be negative nor higher than the input length");

            Name = name;
            Input = input;
            Begin = begin;
            MatchedRule = matchedRule;
            End = input.Length;
            ChildLeafs = new List<Node>();
        }

        internal int Begin { get; set; }
        internal int End { get; set; }
        internal string Input { get; set; }
        internal int Length => End > Begin ? End - Begin : 0;
        internal IList<Node> ChildLeafs { get; set; }

        public string Name { get; }
        public IList<Node> Leafs => new ReadOnlyCollection<Node>(ChildLeafs);
        public string Text => Input.Substring(Begin, Length);
        public bool IsLeaf => !ChildLeafs.Any();
        public Rule MatchedRule { get; }
        public override string ToString() => $"{Name}: {Text}";
    }
}