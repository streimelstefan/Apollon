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
    public class ResolutionBaseState : ICloneable
    {

        public Statement[] Statements { get; private set; }

        public Stack<Literal> CallStack { get; private set; }

        public CHS Chs { get; set; }

        public List<Term> KeepUnbound { get; private set; } = new List<Term>();

        public ILogger Logger { get; private set; }
        public Substitution Substitution { get; set; }

        public ResolutionBaseState(Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
        {
            Statements = statements;
            CallStack = callStack;
            Chs = chs;
            Logger = logger;
            Substitution = substitution;
        }

        public ResolutionBaseState(Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, List<Term> keepUnbound, ILogger logger)
        {
            Statements = statements;
            CallStack = callStack;
            Chs = chs;
            Logger = logger;
            Substitution = substitution;
            KeepUnbound = keepUnbound;
        }

        public virtual object Clone()
        {
            return new ResolutionBaseState(
                Statements.Select(s => (Statement)s.Clone()).ToArray(),
                new Stack<Literal>(CallStack.Select(l => (Literal)l.Clone()).Reverse()),
                (CHS)Chs.Clone(),
                Substitution.Clone(),
                new List<Term>(this.KeepUnbound.Select(l => (Term)l.Clone())),
                Logger.CreateChild());
        }

        public virtual void LogState()
        {
            Logger.Silly($"State:");
            Logger.Silly($"CHS: {Chs}");
            Logger.Silly($"Callstack: [{string.Join(", ", CallStack.Select(l => l.ToString()))}]");
            Logger.Silly($"Substitution: {Substitution}");
            Logger.Silly($"Keep Unbound: [{string.Join(", ", KeepUnbound.Select(l => l.ToString()))}]");
        }
    }
}
