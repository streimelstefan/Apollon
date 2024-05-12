using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers.CallStack
{
    public class CallStackChecker : ICallStackChecker
    {
        private IUnifier _exactUnifer = new ExactUnifier();
        private IUnifier _constructiveUnifier = new ConstructiveUnifier();

        public CheckerResult CheckCallStackFor(Literal literal, Stack<Literal> stack)
        {
            // if there is no loop continue.
            if (!stack.Where(l => _constructiveUnifier.Unify(literal, l).IsSuccess).Any()) return CheckerResult.Continue;
            var exactCallStack = stack.TakeWhile(l => _exactUnifer.Unify(l, literal).IsError).ToList();

            if (IsPositiveLoop(exactCallStack))
            {
                return CheckerResult.Fail;
            }
            if (IsEvenLoop(exactCallStack))
            {
                return CheckerResult.Succeed;
            }

            var constructiveCallStack = stack.TakeWhile(l => _constructiveUnifier.Unify(l, literal).IsError).ToList();
            if (IsEvenLoop(constructiveCallStack))
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

    }
}
