using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CallStackAndCHS
{
    public class CoinductiveCHSChecker : ICoinductiveCHSChecker
    {

        private IUnifier _unifer;

        public CoinductiveCHSChecker(IUnifier unifier)
        {
            _unifer = unifier;
        }

        public CCHSResult CheckCHSFor(Literal literal, CHS chs)
        {
            if (IsPresentWithNAFSwitch(literal, chs))
            {
                return CCHSResult.Fail;
            }

            // if there is no loop continue.
            if (!chs.Literals.Where(l => _unifer.Unify(literal, l).IsSuccess).Any()) return CCHSResult.Continue;
            var checkingChs = chs.Literals.TakeWhile(l => _unifer.Unify(l, literal).IsError).ToList();


            if (IsEvenLoop(checkingChs))
            {
                return CCHSResult.Succeed;
            }
            if (IsPositiveLoop(checkingChs))
            {
                return CCHSResult.Fail;
            }


            return CCHSResult.Continue;
        }

        private bool IsEvenLoop(IEnumerable<Literal> chs)
        {
            var nafCount = chs.Where(l => l.IsNAF).Count();
            return nafCount % 2 == 0 && nafCount != 0;
        }

        private bool IsPositiveLoop(IEnumerable<Literal> chs)
        {
            return !chs.Where(l => l.IsNAF).Any();
        }

        private bool IsPresentWithNAFSwitch(Literal literal, CHS chs)
        {
            var copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Literals.Where(l => _unifer.Unify(l, copy).IsSuccess).Any();
        }
    }
}
