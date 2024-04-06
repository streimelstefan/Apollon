using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.CallGraph;

public class CallGraphEdge
{
    public CallGraphNode Source { get; set; }
    public CallGraphNode Target { get; set; }
    public int Weight { get; set; }
    public Rule CreatorRule { get; set; } // creating a UUID might be a better idea.

    public CallGraphEdge(CallGraphNode source, CallGraphNode target, int weight, Rule creatorRule)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source), "Source can not be null!");
        Target = target ?? throw new ArgumentNullException(nameof(target), "Source can not be null!");
        CreatorRule = creatorRule ?? throw new ArgumentNullException(nameof(CreatorRule), "Rule can not be null!");
        
        if (weight < 0 || weight > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Weight), "Weight must be either 0 or 1.");
        }
        Weight = weight;
    }
}
