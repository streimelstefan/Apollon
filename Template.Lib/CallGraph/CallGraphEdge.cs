using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.Graph;

public class CallGraphEdge
{
    public CallGraphNode Source { get; set; }
    public CallGraphNode Target { get; set; }
    public bool IsNAF { get; set; }
    public Rule CreatorRule { get; set; }

    public CallGraphEdge(CallGraphNode source, CallGraphNode target, bool isNaf, Rule creatorRule)
    {
        Source = source;
        Target = target;
        CreatorRule = creatorRule;
        IsNAF = isNaf;
    }
}
