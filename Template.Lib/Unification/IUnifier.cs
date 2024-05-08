using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public interface IUnifier
    {

        UnificationResult Unify(Literal unifier, Literal against, ISubstitution sigma);
        UnificationResult Unify(BodyPart unifier, BodyPart against, ISubstitution sigma);
        UnificationResult Unify(Literal unifier, Literal against);
        UnificationResult Unify(BodyPart unifier, BodyPart against);

    }
}
