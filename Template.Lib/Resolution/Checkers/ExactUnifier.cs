using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers
{
    public class ExactUnifier : IUnifier
    {


        public UnificationResult Unify(Literal unifier, Literal against, ISubstitution sigma)
        {
            if (unifier.Equals(against))
            {
                return new UnificationResult(new Substitution());
            }
            return new UnificationResult("Are not exact match");
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against, ISubstitution sigma)
        {
            if (unifier.Equals(against))
            {
                return new UnificationResult(new Substitution());
            }
            return new UnificationResult("Are not exact match");
        }

        public UnificationResult Unify(Literal unifier, Literal against)
        {
            if (unifier.Equals(against))
            {
                return new UnificationResult(new Substitution());
            }
            return new UnificationResult("Are not exact match");
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against)
        {
            if (unifier.Equals(against))
            {
                return new UnificationResult(new Substitution());
            }
            return new UnificationResult("Are not exact match");
        }
    }
}
