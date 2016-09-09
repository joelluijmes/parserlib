using System.Collections.Generic;
using ParserLib.Parsing.Rules;

namespace ParserLib.Parsing
{
    public sealed class SharedGrammar : Grammar
    {
        protected override IEnumerable<Rule> GetGrammarRules()
        {
            throw new System.NotImplementedException();
        }
    }
}