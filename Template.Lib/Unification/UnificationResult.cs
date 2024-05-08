using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class UnificationResult : Maybe<ISubstitution, string>
    {
        public UnificationResult(ISubstitution value) : base(value)
        {
        }

        public UnificationResult(string error) : base(error)
        {
        }

        public UnificationResult(ISubstitution? value, string? error) : base(value, error)
        {
        }
    }
}
