using Apollon.Lib.Atoms;
using Apollon.Lib.Resolution;
using Apollon.Lib.Resolution.CoSLD;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib
{
    public class ResultStringBuilder
    {

        public string CreateResultString(ResolutionResult res)
        {
            StringBuilder result = new StringBuilder();

            if (!res.Success)
            {
                result.AppendLine("No solution found.");
                return result.ToString();
            }

            result.AppendLine("Solution found:\n");
            result.AppendLine("{");

            foreach (var literal in res.CHS.Literals)
            {
                result.Append("  ");
                result.Append(BuildLiteralString(literal));
                result.AppendLine();
            }
            result.AppendLine("}");

            var subRes = BuildSubstitutionString(res.Substitution);

            if (subRes.Length > 0)
            {
                result.AppendLine();
                result.Append(subRes);
            }


            return result.ToString();
        }

        private StringBuilder BuildSubstitutionString(ISubstitution sub)
        {
            var res = new StringBuilder();

            foreach (var mapping in sub.Mappings)
            {
                if (mapping.MapsTo.Term != null && mapping.MapsTo.Term.IsVariable)
                {
                    var variable = mapping.MapsTo.Term;

                    if (variable.ProhibitedValues.GetValues().Count() == 0)
                    {
                        // skip entries that link to normal variables.
                        continue;
                    }

                    foreach (var pvl in variable.ProhibitedValues.GetValues())
                    {
                        res.Append(mapping.Variable.Value);
                        res.Append(" != ");
                        res.Append(BuildAtomParamString(pvl, new StringBuilder()));
                        res.AppendLine();
                    }
                } else
                {
                    res.Append(mapping.Variable.Value);
                    res.Append(" = ");
                    res.Append(BuildAtomParamString(mapping.MapsTo, new StringBuilder()));
                    res.AppendLine();
                }
            }

            return res;
        }

        private StringBuilder BuildLiteralString(Literal literal)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder pvl = new StringBuilder();

            if (literal.IsNAF)
                result.Append("not ");

            if (literal.IsNegative)
                result.Append("-");

            result.Append(BuildAtomString(literal.Atom, pvl));

            if (pvl.Length > 0)
            {
                result.Append(" (");
                result.Append(pvl);
                result.Append(")");
            }

            return result;
        }

        private StringBuilder BuildAtomString(Atom atom, StringBuilder pvlStringBuilder)
        {
            StringBuilder res = new StringBuilder();

            res.Append(atom.Name);

            if (atom.ParamList.Count() == 0)
            {
                return res;
            }

            res.Append('(');
            for (int i = 0; i < atom.ParamList.Count() - 1; i++)
            {
                var param = atom.ParamList[i];
                res.Append(BuildAtomParamString(param, pvlStringBuilder));
                res.Append(", ");
            }
            res.Append(BuildAtomParamString(atom.ParamList.Last(), pvlStringBuilder));

            res.Append(")");
            return res;
        }

        private StringBuilder BuildAtomParamString(AtomParam atomParam, StringBuilder pvlStringBuilder)
        {
            var res = new StringBuilder();
            if (atomParam.Term != null)
            {
                res.Append(atomParam.Term.Value);

                foreach (var pvl in atomParam.Term.ProhibitedValues.GetValues())
                {
                    if (pvlStringBuilder.Length > 0)
                        pvlStringBuilder.Append(", ");

                    pvlStringBuilder.Append(atomParam.Term.Value);
                    pvlStringBuilder.Append(" != ");
                    pvlStringBuilder.Append(BuildAtomParamString(pvl, pvlStringBuilder));
                }
            }

            if (atomParam.Literal != null)
            {
                res.Append(BuildLiteralString(atomParam.Literal));
            }

            return res;
        }

    }
}
