using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib
{
    public class Program
    {

        public Literal[] LiteralList { get; private set; }

        public Rule[] RuleList { get; private set; }

        public Constraint[] ConstraintList { get; private set; }

        public IEnumerable<Statement> RuleTypesAsStatements
        {
            get
            {
                return RuleList.Select(r => r as Statement)
                    .Union(ConstraintList.Select(c => c as Statement));
            }
        }

        public IEnumerable<Statement> Statements
        {
            get
            {
                return LiteralList.Select(l => new Statement(l, new Literal[0]))
                    .Union(RuleTypesAsStatements);
            }
        }

        public Program(Literal[] literalList, Rule[] ruleList, Constraint[] constraints)
        { 
            LiteralList = literalList;
            RuleList = ruleList;
            ConstraintList = constraints;
        }

        public Program()
        {
            LiteralList = new Literal[0];
            RuleList = new Rule[0];
            ConstraintList = new Constraint[0];
        }

    }
}
