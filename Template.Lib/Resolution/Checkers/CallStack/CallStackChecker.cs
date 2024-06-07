//-----------------------------------------------------------------------
// <copyright file="CallStackChecker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers.CallStack
{
    using Apollon.Lib.Linker;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;

    /// <summary>
    /// The CallStackChecker checks the CallStack for loops that allows the Co SLD Resolution to either
    /// fail or suceed early.
    /// </summary>
    public class CallStackChecker : ICallStackChecker
    {
        private readonly IUnifier exactUnifer = new ExactUnifier();
        private readonly IUnifier constructiveUnifier = new ConstructiveUnifier();

        private readonly VariableLinker linker = new VariableLinker();
        private readonly VariableExtractor extractor = new VariableExtractor();

        /// <summary>
        /// Checks the CallStack for loops.
        /// </summary>
        /// <param name="literal">The Literal that the loop should be checked for.</param>
        /// <param name="stack">The stack of all Literals that should be checked.</param>
        /// <returns>Returns an Enumerable containing the Result of the Check.</returns>
        public CheckerResult CheckCallStackFor(Literal literal, Stack<Literal> stack)
        {
            // if there is no loop continue.
            if (this.IsPresentWithNAFSwitch(literal, stack))
            {
                return CheckerResult.Fail;
            }

            if (!stack.Where(l => this.constructiveUnifier.Unify(literal, l).IsSuccess).Any())
            {
                return CheckerResult.Continue;
            }

            List<Literal> exactCallStack = stack.TakeWhile(l => this.exactUnifer.Unify(l, literal).IsError).ToList();

            if (this.IsPositiveLoop(exactCallStack))
            {
                return CheckerResult.Fail;
            }

            if (this.IsEvenLoop(exactCallStack))
            {
                return CheckerResult.Succeed;
            }

            this.linker.LinkVariables(new Statement(literal));
            var variables = this.extractor.ExtractVariablesFrom(literal);

            List<Literal> constructiveCallStack = stack.TakeWhile(l => this.constructiveUnifier.Unify(l, literal).IsError)
                .Where(l =>
            {
                this.linker.LinkVariables(new Statement(l));
                var lVars = this.extractor.ExtractVariablesFrom(literal);

                return !lVars.Where(lv => variables.Where(v => v.Value == lv.Value).Any()).Any();
            }).ToList();

            return this.IsEvenLoop(constructiveCallStack) ? CheckerResult.Succeed : CheckerResult.Continue;
        }

        private bool IsEvenLoop(IEnumerable<Literal> chs)
        {
            int nafCount = chs.Where(l => l.IsNAF).Count();
            return nafCount % 2 == 0 && nafCount != 0;
        }

        private bool IsPositiveLoop(IEnumerable<Literal> chs)
        {
            return !chs.Where(l => l.IsNAF).Any() && chs.Count() > 0;
        }

        private bool IsPresentWithNAFSwitch(Literal literal, IEnumerable<Literal> chs)
        {
            Literal copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Where(l => this.exactUnifer.Unify(l, copy).IsSuccess).Any();
        }
    }
}
