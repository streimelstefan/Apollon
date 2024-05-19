using Apollon.Lib.Atoms;
using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class ExactUnifier : IUnifier
    {


        public UnificationResult Unify(Literal unifier, Literal against, ISubstitution sigma)
        {

            return Unify(unifier, against);
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against, ISubstitution sigma)
        {
            return Unify(unifier, against);
        }

        public UnificationResult Unify(Literal unifier, Literal against)
        {
            var res = Unify(unifier.Atom, against.Atom);
            if (res.IsError)
            {
                return res;
            }

            if (unifier.IsNAF != against.IsNAF)
            {
                return new UnificationResult($"NAF is not the same | {unifier} \\= {against}");
            }
            if (unifier.IsNegative != against.IsNegative)
            {
                return new UnificationResult($"Negation is not the same | {unifier} \\= {against}");
            }

            return res;
        }

        public UnificationResult Unify(BodyPart unifier, BodyPart against)
        {
            if (unifier.IsLiteral != against.IsLiteral || unifier.IsForAll != against.IsForAll || 
                unifier.HasChild != against.HasChild || unifier.IsOperation != unifier.IsOperation)
            {
                return new UnificationResult($"Type does not match | {unifier} \\= {against}");
            }

            // here for all can be ignored since the forall term needs to be varaible anyway

            if (unifier.Literal != null && against.Literal != null)
            {
                return Unify(unifier.Literal, against.Literal);
            }

            if (unifier.Operation != null && against.Operation != null)
            {
                return Unify(unifier.Operation, against.Operation);
            }

            if (unifier.Child != null && against.Child != null)
            {
                return Unify(unifier.Child, against.Child);
            }

            throw new InvalidOperationException($"Unahndled Case within Exact Unification => {unifier} {against}");
        }

        public UnificationResult Unify(Atom atom1, Atom atom2)
        {
            if (atom1.Name != atom2.Name)
                return new UnificationResult($"Name missmatch => {atom1} \\= {atom2}");

            if (atom1.ParamList.Length != atom2.ParamList.Length)
                return new UnificationResult($"ParamList length missmatch => {atom1} \\= {atom2}");

            for (int i = 0; i < atom1.ParamList.Length; i++)
            {
                var res = Unify(atom1.ParamList[i], atom2.ParamList[i]);
                if (res.IsError)
                {
                    return res;
                }
            }

            return new UnificationResult(new Substitution());
        }

        public UnificationResult Unify(AtomParam param1, AtomParam param2)
        {
            if (param1.IsTerm != param2.IsTerm || param2.IsLiteral != param1.IsLiteral)
                return new UnificationResult($"Type missmatch => {param1} \\= {param2}");

            if (param1.Literal != null && param2.Literal != null)
                return Unify(param1.Literal, param2.Literal);
            
            if (param1.Term != null && param2.Term != null)
            {
                return Unify(param1.Term, param2.Term);
            }

            throw new InvalidOperationException($"Unhandled Case in Exact Unification => {param1} {param2}");
        }

        public UnificationResult Unify(Operation op1, Operation op2)
        {
            var res = Unify(op1.Variable, op2.Variable);
            if (res.IsError)
                return res;

            if (op1.Operator != op2.Operator)
                return new UnificationResult($"Operation missmatch => {op1} \\= {op2}");

            return Unify(op1.Condition, op2.Condition);
        }

        public UnificationResult Unify(Term term1, Term term2)
        {
            if (term1.IsVariable != term2.IsVariable)
                return new UnificationResult($"Type missmatch => {term1} \\= {term2}");
            if (term1.IsVariable && term2.IsVariable)
            {
                var pvl1 = term1.ProhibitedValues.GetValues().ToArray();
                var pvl2 = term2.ProhibitedValues.GetValues().ToArray();

                // if both pvls are equal succeed if not fail.
                if (pvl1.Length != pvl2.Length)
                {
                    return new UnificationResult($"PVL length missmatch => {term1} \\= {term2}");
                }

                for (int i = 0; i < pvl1.Length; i++)
                {
                    var res = Unify(pvl1[i], pvl2[i]);
                    if (res.IsError) return res;
                }

                return new UnificationResult(new Substitution());
            }

            if (term1.Value != term2.Value)
                return new UnificationResult($"Name missmatch => {term1} \\= {term2}");

            return new UnificationResult(new Substitution());
        }
    }
}
