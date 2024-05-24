using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public interface IUnifier
    {

        UnificationResult Unify(Literal unifier, Literal against, Substitution sigma);
        UnificationResult Unify(BodyPart unifier, BodyPart against, Substitution sigma);
        UnificationResult Unify(Literal unifier, Literal against);
        UnificationResult Unify(BodyPart unifier, BodyPart against);

    }
}
