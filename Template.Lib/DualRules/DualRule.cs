using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Rules;

namespace Apollon.Lib.DualRules
{
    /// <summary>
    /// Represents a dual rule in a logic program. This statement enforces NAF in the head.
    /// </summary>
    public class DualRule : Statement
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DualRule"/> class.
        /// </summary>
        /// <param name="head">The head of the dual rule.</param>
        /// <param name="body">The body of the dual rule</param>
        /// <exception cref="ArgumentNullException">Is thrown if the head is null></exception>
        /// <exception cref="ArgumentException">Is thrown fi the head is NAF></exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown if the body contains no elements.</exception>
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
