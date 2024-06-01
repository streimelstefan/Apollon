//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    using Apollon.Lib.Docu;
    using Apollon.Lib.Rules;

    /// <summary>
    /// This class represents a ASP Program after the lexing and parsing process.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="literalList">An array of all Literals.</param>
        /// <param name="ruleList">An array of all Rules.</param>
        /// <param name="constraints">An array of all Constraints.</param>
        /// <param name="documentation">An array of all Documenatation parts.</param>
        public Program(Literal[] literalList, Rule[] ruleList, Constraint[] constraints, IDocumentation[] documentation)
        {
            this.LiteralList = literalList;
            this.RuleList = ruleList;
            this.ConstraintList = constraints;
            this.Documentation = documentation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        public Program()
            : this(new Literal[0], new Rule[0], new Constraint[0], new Documentation[0])
        {
        }

        /// <summary>
        /// Gets an array of all Literals in the Program.
        /// </summary>
        public Literal[] LiteralList { get; private set; }

        /// <summary>
        /// Gets an array of all Rules in the Program.
        /// </summary>
        public Rule[] RuleList { get; private set; }

        /// <summary>
        /// Gets an array of all Constraints in the Program.
        /// </summary>
        public Constraint[] ConstraintList { get; private set; }

        /// <summary>
        /// Gets an array of all Documentation in the Program.
        /// </summary>
        public IDocumentation[] Documentation { get; private set; }

        /// <summary>
        /// Gets an Enumerable of all RuleTypes as Statements.
        /// </summary>
        public IEnumerable<Statement> RuleTypesAsStatements => this.RuleList.Select(r => r as Statement)
                    .Union(this.ConstraintList.Select(c => c as Statement));

        /// <summary>
        /// Gets an Enumerable of all Statements in the Program.
        /// </summary>
        public IEnumerable<Statement> Statements => this.LiteralList.Select(l => new Statement(l, new BodyPart[0]))
                    .Union(this.RuleTypesAsStatements);

        /// <summary>
        /// Gets an Enumerable of all Literals in the Program.
        /// </summary>
        public IEnumerable<Literal> AllLiterals =>
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                this.Statements
                    .SelectMany(s => s.Body.Where(b => b.Literal != null).Select(b => b.Literal))
                    .Union(this.Statements.Where(s => s.Head != null).Select(s => s.Head));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    }
}
