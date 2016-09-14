﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing
{
    public class Node
    {
        public Node(string name, string input, int begin)
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
            End = input.Length;
            Leafs = new List<Node>();
        }

        public int Begin { get; set; }
        public IList<Node> Leafs { get; set; }
        public int End { get; set; }
        public string Input { get; set; }

        public string Name { get; }

        public int Length => End > Begin ? End - Begin : 0;
        public string Text => Input.Substring(Begin, Length);
        public bool IsLeaf => !Leafs.Any();
        public Node FirstLeaf => Leafs.First();
        public int ChildCount => Leafs.Count;
        public override string ToString() => $"{Name}: {Text}";
    }
}