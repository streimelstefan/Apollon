//-----------------------------------------------------------------------
// <copyright file="ResolutionRecursionState.cs" company="Streimel and Prix">
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
    /// The Resolution Recursion State.
    /// </summary>
    public class ResolutionRecursionState : ResolutionBaseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="goals">All goals.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionRecursionState(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
            : base(statements, callStack, chs, substitution, logger)
        {
            this.Goals = goals;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="goals">All goals.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="keepUnbound">List of Terms that should be kept unbound.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionRecursionState(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
            : base(statements, callStack, chs, substitution, keepUnbound, logger)
        {
            this.Goals = goals;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States.</param>
        /// <param name="goals">The goals.</param>
        public ResolutionRecursionState(ResolutionBaseState baseState, BodyPart[] goals)
            : this(goals, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.KeepUnbound, baseState.Logger)
        {
        }

        /// <summary>
        /// Gets all current goals.
        /// </summary>
        public BodyPart[] Goals { get; private set; }

        public Dictionary<string, List<Literal>> BodyOnlyLiteralAndVars { get; set; } = new Dictionary<string, List<Literal>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="goals">All goals.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A new cloned instance.</returns>
        public static ResolutionRecursionState CloneConstructor(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
        {
            ResolutionRecursionState obj = new(goals, statements, callStack, chs, substitution, logger);

            return (ResolutionRecursionState)obj.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="goals">All goals.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="keepUnbound">List of Terms that should be kept unbound.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A new cloned instance.</returns>
        public static ResolutionRecursionState CloneConstructor(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
        {
            ResolutionRecursionState obj = new(goals, statements, callStack, chs, substitution, keepUnbound, logger);

            return (ResolutionRecursionState)obj.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionRecursionState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States that should be used in the clone.</param>
        /// <param name="goals">The goals that should be used in the clone.</param>
        /// <returns>A new cloned instance.</returns>
        public static ResolutionRecursionState CloneConstructor(ResolutionBaseState baseState, BodyPart[] goals)
        {
            ResolutionRecursionState obj = new(baseState, goals);

            return (ResolutionRecursionState)obj.Clone();
        }


        /// <summary>
        /// Clones the current State.
        /// </summary>
        /// <returns>A cloned object of the current state.</returns>
        public override object Clone()
        {
            ResolutionBaseState baseObj = (ResolutionBaseState)base.Clone();

            var boundCopy = new Dictionary<string, List<Literal>>(
                this.BodyOnlyLiteralAndVars.Select(kv =>
                new KeyValuePair<string, List<Literal>>(kv.Key, kv.Value.Select(l => (Literal)l.Clone()).ToList())));

            var res = new ResolutionRecursionState(baseObj, this.Goals.Select(g => (BodyPart)g.Clone()).ToArray());

            res.BodyOnlyLiteralAndVars = boundCopy;
            return res;
        }
    }
}
