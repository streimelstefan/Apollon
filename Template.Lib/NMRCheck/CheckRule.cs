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
    /// <summary>
    /// A NMR Check rule.
    /// </summary>
    /// <param name="head">The head of the rule</param>
    /// <param name="body">The body of the rule</param>
    /// <exception cref="ArgumentNullException">Is thrown if the head of the rule is null.</exception>
    /// <exception cref="ArgumentException">Is thrown if the head of the rule is not NAF.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if the body contains no elements.</exception>
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
