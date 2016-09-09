using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing
{
    public sealed class Node
    {
        public Node(string name, string input, int begin, int end = 0, IEnumerable<Node> children = null)
        {
            Name = name;
            Input = input;
            Begin = begin;
            End = end;
            Childs = new List<Node>(children ?? new Node[0]);
        }

        public int Begin { get; set; }
        public IList<Node> Childs { get; set; }
        public int End { get; set; }
        public string Input { get; set; }

        public string Name { get; }

        public int Length => End > Begin ? End - Begin : 0;
        public string Text => Input.Substring(Begin, Length);
        public bool IsLeaf => !Childs.Any();
        public int ChildCount => Childs.Count;
        public override string ToString() => $"{Name}: {Text}";
    }
}