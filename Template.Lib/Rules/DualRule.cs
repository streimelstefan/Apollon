using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class DualRules : Statement
    {

        public DualRules(Literal head, params BodyPart[] body) : base(head, body)
        {
            if (Head == null) throw new ArgumentNullException(nameof(head));
            if (Body.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Body), Body.Length, "Body needs to have at least one literal.");
            }
        }
    }
}
