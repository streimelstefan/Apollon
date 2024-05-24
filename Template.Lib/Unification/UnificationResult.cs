using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class UnificationResult : Maybe<Substitution, string>
    {
        public UnificationResult(Substitution value) : base(value)
        {
        }

        public UnificationResult(string error) : base(error)
        {
        }

        public UnificationResult(Substitution? value, string? error) : base(value, error)
        {
        }
    }
}
