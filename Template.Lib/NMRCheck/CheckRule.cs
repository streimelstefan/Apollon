using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apollon.Lib.NMRCheck;

public class CheckRule : Statement
{
    public CheckRule(Literal head, params BodyPart[] body) : base(head, body)
    {
        if (Head == null) throw new ArgumentNullException(nameof(head));
        if (!Head.IsNAF) throw new ArgumentException("Head of a NMR Check rule needs to be NAF.");
        if (Body.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Body), Body.Length, "Body needs to have at least one literal.");
        }
    }

    public override string ToString()
    {
        return $"{Head} :- {string.Join(", ", Body?.Select(literal => literal?.ToString()))}.";
    }
}
