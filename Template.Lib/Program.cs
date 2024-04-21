using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Rules;

namespace Template.Lib
{
    public class Program
    {

        public Literal[] LiteralList { get; set; }

        public Rule[] RuleList { get; set; }

        public Program(Literal[] literalList, Rule[] ruleList)
        { 
            LiteralList = literalList;
            RuleList = ruleList;
        }

        public Program()
        {
            LiteralList = new Literal[0];
            RuleList = new Rule[0];
        }

    }
}
