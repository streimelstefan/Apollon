using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Atoms;

namespace Apollon.Lib.Disunification
{
    public interface IDisunifier
    {
        UnificationResult Disunify(AtomParam unifier, AtomParam against, Substitution sigma);
        UnificationResult Disunify(AtomParam unifier, AtomParam against);

    }
}
