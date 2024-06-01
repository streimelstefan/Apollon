//-----------------------------------------------------------------------
// <copyright file="ResolutionBaseState.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.CoSLD.States
{
    using Apollon.Lib.Logging;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The Resolution Base State.
    /// </summary>
    public class ResolutionBaseState : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionBaseState"/> class.
        /// </summary>
        /// <param name="statements">An array of all Statements.</param>
        /// <param name="callStack">The callstack of all literals.</param>
        /// <param name="chs">The CHS for the resolution.</param>
        /// <param name="substitution">The substitution for the current CHS.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionBaseState(Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
        {
            this.Statements = statements;
            this.CallStack = callStack;
            this.Chs = chs;
            this.Logger = logger;
            this.Substitution = substitution;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionBaseState"/> class.
        /// </summary>
        /// <param name="statements">An array of all Statements.</param>
        /// <param name="callStack">The callstack of all literals.</param>
        /// <param name="chs">The CHS for the resolution.</param>
        /// <param name="substitution">The substitution for the current CHS.</param>
        /// <param name="keepUnbound">The List of Terms that should be kept unbound.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionBaseState(Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
        {
            this.Statements = statements;
            this.CallStack = callStack;
            this.Chs = chs;
            this.Logger = logger;
            this.Substitution = substitution;
            this.KeepUnbound = keepUnbound;
        }

        /// <summary>
        /// Gets the Array of Statements.
        /// </summary>
        public Statement[] Statements { get; private set; }

        /// <summary>
        /// Gets the CallStack.
        /// </summary>
        public Stack<Literal> CallStack { get; private set; }

        /// <summary>
        /// Gets or sets the CHS.
        /// </summary>
        public CHS Chs { get; set; }

        /// <summary>
        /// Gets the List of Terms that should be kept unbound.
        /// </summary>
        public List<Term> KeepUnbound { get; private set; } = new List<Term>();

        /// <summary>
        /// Gets the Logger.
        /// </summary>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Gets or sets the Substitution.
        /// </summary>
        public Substitution Substitution { get; set; }

        /// <summary>
        /// Clones the ResolutionBaseState.
        /// </summary>
        /// <returns>Returns a clone of this state.</returns>
        public virtual object Clone()
        {
            return new ResolutionBaseState(
                this.Statements.Select(s => (Statement)s.Clone()).ToArray(),
                new Stack<Literal>(this.CallStack.Select(l => (Literal)l.Clone()).Reverse()),
                (CHS)this.Chs.Clone(),
                this.Substitution.Clone(),
                new List<Term>(this.KeepUnbound.Select(l => (Term)l.Clone())),
                this.Logger.CreateChild());
        }

        /// <summary>
        /// Logs the State.
        /// </summary>
        public virtual void LogState()
        {
            this.Logger.Silly($"State:");
            this.Logger.Silly($"CHS: {this.Chs}");
            this.Logger.Silly($"Callstack: [{string.Join(", ", this.CallStack.Select(l => l.ToString()))}]");
            this.Logger.Silly($"Substitution: {this.Substitution}");
            this.Logger.Silly($"Keep Unbound: [{string.Join(", ", this.KeepUnbound.Select(l => l.ToString()))}]");
        }
    }
}
