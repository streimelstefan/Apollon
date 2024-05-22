using Apollon.Lib.Logging;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD.States
{
    public class ResolutionBaseState
    {

        public Statement[] Statements { get; private set; }

        public Stack<Literal> CallStack { get; private set; }

        public CHS Chs { get; private set; }

        public ILogger Logger { get; private set; }
        public ISubstitution Substitution { get; private set; }

        public ResolutionBaseState(Statement[] statements, Stack<Literal> callStack, CHS chs, ISubstitution substitution, ILogger logger)
        {
            Statements = statements;
            CallStack = callStack;
            Chs = chs;
            Logger = logger;
            Substitution = substitution;
        }
    }
}
