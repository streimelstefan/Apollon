using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers.CallStack
{
    public interface ICallStackChecker
    {
        CheckerResult CheckCallStackFor(Literal literal, Stack<Literal> stack);
    }
}
