using Apollon.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.OLON
{
    public class OlonSet
    {

        public HashSet<CallGraphNode> Nodes = new HashSet<CallGraphNode>();

        public bool IsPartOfOlon(CallGraphNode node)
        {
            return Nodes.Contains(node);
        }

    }
}
