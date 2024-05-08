using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification.DisagreementFinders
{
    public class DisagreementResult : Maybe<Disagreement, string>
    {
        public DisagreementResult(Disagreement value) : base(value)
        {
        }

        public DisagreementResult(string error) : base(error)
        {
        }

        public DisagreementResult(Disagreement? value, string? error) : base(value, error)
        {
        }
    }
}
