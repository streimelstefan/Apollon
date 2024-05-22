using Apollon.Lib.Logging;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class ResolutionStepState
    {

        public Statement[] Statements { get; private set; }
        
        public Literal Goal { get; private set; }
        
        public Stack<Literal> CallStack { get; private set; }

        public CHS Chs { get; private set; }

        public ILogger Logger { get; private set; }


    }
}
