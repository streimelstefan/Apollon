using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.CallGraph
{
    public class CallGraphNode
    {

        public Literal Literal { get; set; }

        public CallGraphNode(Literal literal)
        {
            Literal = literal;
        }
    }
}
