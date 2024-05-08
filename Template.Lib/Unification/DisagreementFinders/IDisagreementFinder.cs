using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification.DisagreementFinders
{
    public interface IDisagreementFinder
    {
        /// <summary>
        /// Returns the first disagreement that was found between the two statments. If there is an disagreement that is not 
        /// fixable the maybe will be error. A disagreement is not fixable if the disagreement is not part of an <see cref="Atoms.AtomParam"/>
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        DisagreementResult FindDisagreement(Statement s1, Statement s2);
    }
}
