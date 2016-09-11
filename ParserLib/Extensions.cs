﻿using System.Collections.Generic;
using System.Linq;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib
{
    public static class Extensions
    {
        public static string PrettyFormat(this Rule rule) => Util.PrettyFormat(rule, r => r.GetChildren());

        public static string PrettyFormat(this Node node) => Util.PrettyFormat(node, n => n.Leafs);

        public static IEnumerable<string> PrettyFormat(this IEnumerable<Node> nodes) => nodes.Select(PrettyFormat);
    }
}