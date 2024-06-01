//-----------------------------------------------------------------------
// <copyright file="ResultStringBuilder.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    using System.Text;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Resolution;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The class that is responsible for creating a string representation of a ResolutionResult.
    /// </summary>
    public class ResultStringBuilder
    {
        /// <summary>
        /// Creates a string representation of a ResolutionResult.
        /// </summary>
        /// <param name="res">The ResolutionsResult what a string should be created for.</param>
        /// <returns>A string representing the Result.</returns>
        public string CreateResultString(ResolutionResult res)
        {
            StringBuilder result = new();

            if (!res.Success)
            {
                _ = result.AppendLine("No solution found.");
                return result.ToString();
            }

            _ = result.AppendLine("Solution found:\n");
            _ = result.AppendLine("{");

            foreach (Literal literal in res.CHS.Literals)
            {
                _ = result.Append("  ");
                _ = result.Append(this.BuildLiteralString(literal));
                _ = result.AppendLine();
            }

            _ = result.AppendLine("}");

            StringBuilder subRes = this.BuildSubstitutionString(res.Substitution);

            if (subRes.Length > 0)
            {
                _ = result.AppendLine();
                _ = result.Append(subRes);
            }

            return result.ToString();
        }

        private StringBuilder BuildSubstitutionString(Substitution sub)
        {
            StringBuilder res = new();

            foreach (Unification.Mapping mapping in sub.Mappings)
            {
                if (mapping.MapsTo.Term != null && mapping.MapsTo.Term.IsVariable)
                {
                    Term variable = mapping.MapsTo.Term;

                    if (variable.ProhibitedValues.GetValues().Count() == 0)
                    {
                        // skip entries that link to normal variables.
                        continue;
                    }

                    foreach (AtomParam pvl in variable.ProhibitedValues.GetValues())
                    {
                        _ = res.Append(mapping.Variable.Value);
                        _ = res.Append(" != ");
                        _ = res.Append(this.BuildAtomParamString(pvl, new StringBuilder()));
                        _ = res.AppendLine();
                    }
                }
                else
                {
                    _ = res.Append(mapping.Variable.Value);
                    _ = res.Append(" = ");
                    _ = res.Append(this.BuildAtomParamString(mapping.MapsTo, new StringBuilder()));
                    _ = res.AppendLine();
                }
            }

            return res;
        }

        private StringBuilder BuildLiteralString(Literal literal)
        {
            StringBuilder result = new();
            StringBuilder pvl = new();

            if (literal.IsNAF)
            {
                _ = result.Append("not ");
            }

            if (literal.IsNegative)
            {
                _ = result.Append("-");
            }

            _ = result.Append(this.BuildAtomString(literal.Atom, pvl));

            if (pvl.Length > 0)
            {
                _ = result.Append(" (");
                _ = result.Append(pvl);
                _ = result.Append(")");
            }

            return result;
        }

        private StringBuilder BuildAtomString(Atom atom, StringBuilder pvlStringBuilder)
        {
            StringBuilder res = new();

            _ = res.Append(atom.Name);

            if (atom.ParamList.Count() == 0)
            {
                return res;
            }

            _ = res.Append('(');
            for (int i = 0; i < atom.ParamList.Count() - 1; i++)
            {
                AtomParam param = atom.ParamList[i];
                _ = res.Append(this.BuildAtomParamString(param, pvlStringBuilder));
                _ = res.Append(", ");
            }

            _ = res.Append(this.BuildAtomParamString(atom.ParamList.Last(), pvlStringBuilder));

            _ = res.Append(")");
            return res;
        }

        private StringBuilder BuildAtomParamString(AtomParam atomParam, StringBuilder pvlStringBuilder)
        {
            StringBuilder res = new();
            if (atomParam.Term != null)
            {
                _ = res.Append(atomParam.Term.Value);

                foreach (AtomParam pvl in atomParam.Term.ProhibitedValues.GetValues())
                {
                    if (pvlStringBuilder.Length > 0)
                    {
                        _ = pvlStringBuilder.Append(", ");
                    }

                    _ = pvlStringBuilder.Append(atomParam.Term.Value);
                    _ = pvlStringBuilder.Append(" != ");
                    _ = pvlStringBuilder.Append(this.BuildAtomParamString(pvl, pvlStringBuilder));
                }
            }

            if (atomParam.Literal != null)
            {
                _ = res.Append(this.BuildLiteralString(atomParam.Literal));
            }

            return res;
        }
    }
}
