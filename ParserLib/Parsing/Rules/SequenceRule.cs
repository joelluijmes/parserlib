﻿using System.Collections.Generic;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches if all of the child rules are matched. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class SequenceRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SequenceRule" /> class.
        /// </summary>
        /// <param name="firstRule">The first rule.</param>
        /// <param name="secondRule">The second rule.</param>
        /// <param name="rules">The rules.</param>
        public SequenceRule(Rule firstRule, Rule secondRule, params Rule[] rules)
        {
            var allRules = Util.MergeArray(firstRule, secondRule, rules);
            Leafs.AddRange(allRules.SelectMany(FlattenRules));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SequenceRule" /> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public SequenceRule(IEnumerable<Rule> rules)
        {
            Leafs.AddRange(rules.SelectMany(FlattenRules));
        }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => Leafs.Skip(1).Aggregate(FirstLeaf.ToString(), (a, b) => $"{a} + {b}");

        /// <summary>
        ///     Specific rule implementation of the match. Which matches if all rules match.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state)
        {
            var oldState = state.Clone();
            if (Leafs.All(c => c.MatchImpl(state)))
                return true;

            state.Assign(oldState);
            return false;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"({base.ToString()})";

        private static IEnumerable<Rule> FlattenRules(Rule r) => r is SequenceRule
            ? (IEnumerable<Rule>)r.GetLeafs()
            : new[] { r };
    }
}