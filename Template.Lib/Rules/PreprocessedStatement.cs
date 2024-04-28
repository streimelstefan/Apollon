using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class PreprocessedStatement : Statement
    {
        public bool IsOlonRule { get; private set; }
        public bool IsOrdiniaryRule { get; private set; }

        public PreprocessedStatement(Statement statement, bool isOlonRule, bool isOrdiniaryRule) : base(statement.Head, statement.Body)
        {
            IsOlonRule = isOlonRule;
            IsOrdiniaryRule = isOrdiniaryRule;
        }

        public override object Clone()
        {
            return new PreprocessedStatement((Statement)base.Clone(), IsOlonRule, IsOrdiniaryRule);
        }

    }
}
