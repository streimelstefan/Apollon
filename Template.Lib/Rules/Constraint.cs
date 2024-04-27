using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class Constraint : Statement
    {
        public Constraint(params Literal[] body) : base(null, body)
        {
        }
    }
}
