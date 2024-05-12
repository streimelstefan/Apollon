using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Disunification
{
    public class ConstructiveDisunifier : IDisunifier
    {

        public IUnifier _unifier = new Unifier();

        public int _unificationIndex = 0;

        public UnificationResult Disunify(AtomParam unifier, AtomParam against)
        {
            return Disunify(unifier, against, new Substitution());
        }

        public UnificationResult Disunify(AtomParam unifier, AtomParam against, ISubstitution sigma)
        {
            throw new NotImplementedException();
        }


        // private UnificationResult Disunify(Term unifier, Term against, ISubstitution sigma)
        // {
        //     if (unifier.IsVariable && !against.IsVariable)
        //     {
        //         sigma.Add(unifier, new AtomParam(null, new Term($"DUV/{_unificationIndex}", new PVL(new AtomParam[] { new AtomParam(against) }))));
        //         _unificationIndex++;
        //         return new UnificationResult(sigma);
        //     } else if (!unifier.IsVariable && against.IsVariable)
        //     {
        //         sigma.Add(unifier, new AtomParam(null, new Term($"DUV/{_unificationIndex}", new PVL(new AtomParam[] { new AtomParam(against) }))));
        //         _unificationIndex++;
        //         return new UnificationResult(sigma);
        //     } else if (unifier.IsVariable && against.IsVariable)
        //     {
        // 
        //     } else
        //     {
        // 
        //     }
        // }
        // 
        // private UnificationResult Disunify(Term unifier, Literal against, ISubstitution sigma)
        // {
        //     if (unifier.IsVariable)
        //     {
        //         sigma.Add(unifier, new AtomParam(null, new Term($"DUV/{_unificationIndex}", new PVL(new AtomParam[] { new AtomParam(against) }))));
        //         _unificationIndex++;
        //         return new UnificationResult(sigma);
        //     }
        //     else
        //     {
        //         // are already different.
        //         return new UnificationResult(new Substitution());
        //     }
        // }
        // 
        // private UnificationResult Disunify(Literal unifier, Literal against, ISubstitution sigma)
        // {
        // 
        // }
        // 
        // private UnificationResult Disunify(Literal unifier, Term against, ISubstitution sigma)
        // {
        //     return Disunify(against, unifier, sigma);
        // }
    }
}
