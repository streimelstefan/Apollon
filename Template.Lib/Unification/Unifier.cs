using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.DisagreementFinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class Unifier : IUnifier
    {

        private IDisagreementFinder _disagreementFinder = new DisagreementFinder();

        public UnificationResult Unify(Literal unifier, Literal against)
        {
            return Unify(new Statement(unifier), new Statement(against), new Substitution());
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against)
        {
            return Unify(new Statement(null, unifier), new Statement(null, against), new Substitution());
        }

        public UnificationResult Unify(Literal unifier, Literal against, ISubstitution sigma)
        {
            return Unify(new Statement(unifier), new Statement(against), sigma);
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against, ISubstitution sigma)
        {
            return Unify(new Statement(null, unifier), new Statement(null, against), sigma);
        }

        public UnificationResult Unify(Statement unifier, Statement against, ISubstitution sigma)
        {
            while (true)
            {
                var tUnifier = sigma.Apply(unifier);
                var tAgainst = sigma.Apply(against);
        
                if (IsSingleton(tUnifier, tAgainst)) // are equal
                {
                    return new UnificationResult(sigma); // All terms are unified under the current substitution
                }
        
                var disagreementSet = _disagreementFinder.FindDisagreement(tUnifier, tAgainst);
                if (disagreementSet.IsError) // a non fixable disagreement has ocured.
                {
                    return new UnificationResult($"Statments are not unifiable. {disagreementSet.Error}");
                }
                 
                var (s, t) = ChooseTermsToResolve(disagreementSet);
                sigma.Add(s, t); // Add this substitution
            }
        }

        private bool IsSingleton(Statement unifier, Statement against)
        {
            return unifier.Equals(against);
        }

        private (Term, AtomParam) ChooseTermsToResolve(DisagreementResult res)
        {
            if (res.Value == null)
            {
                throw new ArgumentException("Value of result is not allowed to be null.");
            }
            if (res.Value.First == null || res.Value.Second == null)
            {
                throw new ArgumentException("Neither first or second of the result are allowed to be null.");
            }

            if (res.Value.First.Term != null && res.Value.First.Term.IsVariable)
            {
                return (res.Value.First.Term, res.Value.Second);
            }
            else if (res.Value.Second.Term != null && res.Value.Second.Term.IsVariable)
            {
                return (res.Value.Second.Term, res.Value.First);
            }

            throw new InvalidOperationException("Not allowed to end here.");
        }
    }
}
