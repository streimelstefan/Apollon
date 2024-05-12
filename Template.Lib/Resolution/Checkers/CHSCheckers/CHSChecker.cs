using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    public class CHSChecker : ICoinductiveCHSChecker
    {
        private IUnifier _unifer = new ExactUnifier();

        public CheckerResult CheckCHSFor(Literal literal, CHS chs)
        {
            if (IsPresentWithNAFSwitch(literal, chs))
                return CheckerResult.Fail;

            // if the chs contains the literal
            if (chs.Literals.Where(l => _unifer.Unify(literal, l).IsSuccess).Any()) return CheckerResult.Succeed;

            return CheckerResult.Continue;
        }

        private bool IsPresentWithNAFSwitch(Literal literal, CHS chs)
        {
            var copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Literals.Where(l => _unifer.Unify(l, copy).IsSuccess).Any();
        }

    }
}
