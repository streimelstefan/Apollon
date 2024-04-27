using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Graph;

public class CallGraphEdge
{
    public CallGraphNode? Source { get; set; }
    public CallGraphNode Target { get; set; }
    public bool IsNAF { get; set; }
    public Statement CreatorRule { get; set; }

    public CallGraphEdge(CallGraphNode? source, CallGraphNode target, bool isNaf, Statement creatorRule)
    {
        Source = source;
        Target = target;
        CreatorRule = creatorRule;
        IsNAF = isNaf;
    }
}
