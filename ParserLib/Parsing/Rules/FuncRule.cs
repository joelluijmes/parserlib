using System;
using System.Linq;

namespace ParserLib.Parsing.Rules
{
    /// <summary>
    ///     Matches if the Func matches, it should be used when pointing to other rules ie for recursive rules. This class
    ///     cannot be inherited.
    /// </summary>
    /// <seealso cref="ParserLib.Parsing.Rules.Rule" />
    public sealed class FuncRule : Rule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FuncRule" /> class.
        /// </summary>
        /// <param name="ruleFunc">The rule function.</param>
        /// <exception cref="System.ArgumentNullException">ruleFunc</exception>
        public FuncRule(Func<Rule> ruleFunc)
        {
            if (ruleFunc == null)
                throw new ArgumentNullException(nameof(ruleFunc));

            RuleFunc = ruleFunc;
        }

        /// <summary>
        ///     Gets the rule function.
        /// </summary>
        /// <value>The rule function.</value>
        public Func<Rule> RuleFunc { get; }

        /// <summary>
        ///     Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public override string Definition => RuleFunc().Definition;

        /// <summary>
        ///     Specific rule implementation of the match.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if input is matched, <c>false</c> otherwise.</returns>
        protected internal override bool MatchImpl(ParserState state)
        {
            if (!Leafs.Any())
                Leafs.Add(RuleFunc());

            return FirstLeaf.MatchImpl(state);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => Name ?? "recursive";
    }
}