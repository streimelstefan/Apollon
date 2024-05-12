using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class CoResolutionResult
    {
        public bool Success { get; set; }

        public ISubstitution Substitution { get; set; }

        public CoResolutionResult(bool success, ISubstitution substitution)
        {
            Success = success;
            Substitution = substitution;
        }

        public CoResolutionResult() : this(false, new Substitution()) { }
    }
}
