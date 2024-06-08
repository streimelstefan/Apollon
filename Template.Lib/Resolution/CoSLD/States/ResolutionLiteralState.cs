//-----------------------------------------------------------------------
// <copyright file="ResolutionLiteralState.cs" company="Streimel and Prix">
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
    /// The Resolution Literal State.
    /// </summary>
    public class ResolutionLiteralState : ResolutionBaseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionLiteralState"/> class.
        /// </summary>
        /// <param name="currentGoal">The current goal.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="keepUnbound">List of Terms that should be kept unbound.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionLiteralState(Literal currentGoal, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
            : base(statements, callStack, chs, substitution, keepUnbound, logger)
        {
            this.CurrentGoal = currentGoal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionLiteralState"/> class.
        /// </summary>
        /// <param name="baseState">The base state.</param>
        /// <param name="currentGoal">The current goal.</param>
        public ResolutionLiteralState(ResolutionBaseState baseState, Literal currentGoal)
            : this(currentGoal, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.KeepUnbound, baseState.Logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionLiteralState"/> class.
        /// </summary>
        /// <param name="baseState">The base state.</param>
        /// <param name="currentGoal">The current goal.</param>
        /// <param name="statements">An array of all statements.</param>
        public ResolutionLiteralState(ResolutionBaseState baseState, Literal currentGoal, Statement[] statements)
            : this(currentGoal, statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.KeepUnbound, baseState.Logger)
        {
        }

        /// <summary>
        /// Gets the Current Goal.
        /// </summary>
        public Literal CurrentGoal { get; set; }

        public Dictionary<string, List<Literal>> BodyOnlyLiteralAndVars { get; set; } = new Dictionary<string, List<Literal>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionLiteralState"/> class.
        /// </summary>
        /// <param name="baseState">The base State that should be cloned.</param>
        /// <param name="currentGoal">The current goal.</param>
        /// <returns>The cloned State as a new object.</returns>
        public static ResolutionLiteralState CloneConstructor(ResolutionBaseState baseState, Literal currentGoal)
        {
            ResolutionLiteralState obj = new(baseState, currentGoal);
            return (ResolutionLiteralState)obj.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionLiteralState"/> class.
        /// </summary>
        /// <param name="baseState">The base State that should be cloned.</param>
        /// <param name="currentGoal">The current goal.</param>
        /// <param name="statements">The statements.</param>
        /// <returns>The cloned State as a new object.</returns>
        public static ResolutionLiteralState CloneConstructor(ResolutionBaseState baseState, Literal currentGoal, Statement[] statements)
        {
            ResolutionLiteralState obj = new(baseState, currentGoal, statements);
            return (ResolutionLiteralState)obj.Clone();
        }

        /// <summary>
        /// Clones the current state.
        /// </summary>
        /// <returns>The cloned state.</returns>
        public override object Clone()
        {
            ResolutionBaseState baseObj = (ResolutionBaseState)base.Clone();
            var boundCopy = new Dictionary<string, List<Literal>>(
                this.BodyOnlyLiteralAndVars.Select(kv => 
                new KeyValuePair<string, List<Literal>>(kv.Key, kv.Value.Select(l => (Literal)l.Clone()).ToList())));

            var res = new ResolutionLiteralState(baseObj, (Literal)this.CurrentGoal.Clone());
            res.BodyOnlyLiteralAndVars = boundCopy;
            return res;
        }
    }
}
