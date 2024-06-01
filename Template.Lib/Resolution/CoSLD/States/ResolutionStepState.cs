//-----------------------------------------------------------------------
// <copyright file="ResolutionStepState.cs" company="Streimel and Prix">
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
    /// The Resolution Step State.
    /// </summary>
    public class ResolutionStepState : ResolutionBaseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="currentGoal">The current goal.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionStepState(BodyPart currentGoal, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
            : base(statements, callStack, chs, substitution, logger)
        {
            this.CurrentGoal = currentGoal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="currentGoal">The current goal.</param>
        /// <param name="statements">The statements.</param>
        /// <param name="callStack">The callstack.</param>
        /// <param name="chs">The CHS.</param>
        /// <param name="substitution">The substitution that should be used.</param>
        /// <param name="keepUnbound">List of Terms that should be kept unbound.</param>
        /// <param name="logger">The logger.</param>
        public ResolutionStepState(BodyPart currentGoal, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
            : base(statements, callStack, chs, substitution, keepUnbound, logger)
        {
            this.CurrentGoal = currentGoal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States that should be used in the clone.</param>
        /// <param name="currentGoal">The clone that should be used in the clone.</param>
        public ResolutionStepState(ResolutionBaseState baseState, BodyPart currentGoal)
            : this(currentGoal, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.KeepUnbound, baseState.Logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States that should be used in the clone.</param>
        /// <param name="currentGoal">The goals that should be used in the clone.</param>
        /// <param name="statements">The array of statements that should be inserted into the state.</param>
        public ResolutionStepState(ResolutionBaseState baseState, BodyPart currentGoal, Statement[] statements)
            : this(currentGoal, statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.KeepUnbound, baseState.Logger)
        {
        }

        /// <summary>
        /// Gets the current goal.
        /// </summary>
        public BodyPart CurrentGoal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States that should be used in the clone.</param>
        /// <param name="currentGoal">The clone that should be used in the clone.</param>
        /// <returns>Returns a new instance.</returns>
        public static ResolutionStepState CloneConstructor(ResolutionBaseState baseState, BodyPart currentGoal)
        {
            ResolutionStepState obj = new(baseState, currentGoal);

            return (ResolutionStepState)obj.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionStepState"/> class.
        /// </summary>
        /// <param name="baseState">The basic States that should be used in the clone.</param>
        /// <param name="currentGoal">The goals that should be used in the clone.</param>
        /// <param name="statements">The array of statements that should be inserted into the state.</param>
        /// <returns>Returns the cloned State.</returns>
        public static ResolutionStepState CloneConstructor(ResolutionBaseState baseState, BodyPart currentGoal, Statement[] statements)
        {
            ResolutionStepState obj = new(baseState, currentGoal, statements);

            return (ResolutionStepState)obj.Clone();
        }

        /// <summary>
        /// Clones the current state.
        /// </summary>
        /// <returns>A cloned object of this state.</returns>
        public override object Clone()
        {
            ResolutionBaseState baseObj = (ResolutionBaseState)base.Clone();

            return new ResolutionStepState(baseObj, (BodyPart)this.CurrentGoal.Clone());
        }
    }
}
