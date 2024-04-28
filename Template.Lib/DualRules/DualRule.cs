using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Rules;

namespace Apollon.Lib.DualRules
{
    public class DualRule : Statement
    {

        public DualRule(Literal head, params BodyPart[] body) : base(head, body)
        {
            if (Head == null) throw new ArgumentNullException(nameof(head));
            if (!Head.IsNAF) throw new ArgumentException("Head of a dual rule needs to be NAF.");
            if (Body.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Body), Body.Length, "Body needs to have at least one literal.");
            }
        }
    }
}
