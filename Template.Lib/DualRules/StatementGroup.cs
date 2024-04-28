using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.DualRules
{
    public class StatementGroup
    {

        public List<Statement> Statements { get; private set; }

        /// <summary>
        /// The reference literal that represents the head of the first statement that was inserted
        /// into this group. It should be used to see if a given statement belongs in this group or not.
        /// </summary>
        public Literal ReferenceLiteral { get; private set; }

        public StatementGroup(Statement statement) 
        {
            if (statement.Head == null)
            {
                throw new ArgumentNullException(nameof(statement), "The head of a statement is not allowed to be null in a statement group");
            }
            Statements = new List<Statement>();
            ReferenceLiteral = statement.Head;
            Statements.Add(statement);
        }

    }
}
