using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    public interface ICoinductiveCHSChecker
    {
        CheckerResult CheckCHSFor(Literal literal, CHS chs);
    }
}
