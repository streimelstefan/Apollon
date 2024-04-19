using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.Rules
{
    public class PreprocessedRule : Rule
    {

        public bool IsOlonRule { get; private set; }
        public bool IsOrdiniaryRule { get; private set; }

        public PreprocessedRule(Rule rule, bool isOlonRule, bool isOrdiniaryRule) : base(rule.Head, rule.Body)
        {
            IsOlonRule = isOlonRule;
            IsOrdiniaryRule = isOrdiniaryRule;
        }

    }
}
