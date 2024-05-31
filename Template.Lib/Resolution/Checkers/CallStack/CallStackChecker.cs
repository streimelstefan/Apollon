namespace Apollon.Lib.Resolution.Checkers.CallStack
{
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Unification;

    /// <summary>
    /// The CallStackChecker checks the CallStack for loops that allows the Co SLD Resolution to either
    /// fail or suceed early.
    /// </summary>
    public class CallStackChecker : ICallStackChecker
    {
        private IUnifier exactUnifer = new ExactUnifier();
        private IUnifier constructiveUnifier = new ConstructiveUnifier();

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

            var exactCallStack = stack.TakeWhile(l => this.exactUnifer.Unify(l, literal).IsError).ToList();

            if (this.IsPositiveLoop(exactCallStack))
            {
                return CheckerResult.Fail;
            }

            if (this.IsEvenLoop(exactCallStack))
            {
                return CheckerResult.Succeed;
            }

            var constructiveCallStack = stack.TakeWhile(l => this.constructiveUnifier.Unify(l, literal).IsError).ToList();
            if (this.IsEvenLoop(constructiveCallStack))
            {
                return CheckerResult.Succeed;
            }

            return CheckerResult.Continue;
        }

        private bool IsEvenLoop(IEnumerable<Literal> chs)
        {
            var nafCount = chs.Where(l => l.IsNAF).Count();
            return nafCount % 2 == 0 && nafCount != 0;
        }

        private bool IsPositiveLoop(IEnumerable<Literal> chs)
        {
            return !chs.Where(l => l.IsNAF).Any() && chs.Count() > 0;
        }

        private bool IsPresentWithNAFSwitch(Literal literal, IEnumerable<Literal> chs)
        {
            var copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Where(l => this.exactUnifer.Unify(l, copy).IsSuccess).Any();
        }
    }
}
