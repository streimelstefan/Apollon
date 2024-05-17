using Apollon.Lib.Docu;
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

        public IDocumentation[] Documentation {  get; private set; }

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
                return LiteralList.Select(l => new Statement(l, new BodyPart[0]))
                    .Union(RuleTypesAsStatements);
            }
        }

        public IEnumerable<Literal> AllLiterals
        {
            get
            {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Statements
                    .SelectMany(s => s.Body.Where(b => b.Literal != null).Select(b => b.Literal))
                    .Union(Statements.Where(s => s.Head != null).Select(s => s.Head));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }


        public Program(Literal[] literalList, Rule[] ruleList, Constraint[] constraints, IDocumentation[] documentation)
        {
            LiteralList = literalList;
            RuleList = ruleList;
            ConstraintList = constraints;
            Documentation = documentation;

        }

        public Program() : this(new Literal[0], new Rule[0], new Constraint[0], new Documentation[0])
        {
        }



    }
}
