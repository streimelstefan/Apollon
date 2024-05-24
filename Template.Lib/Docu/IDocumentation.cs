using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public interface IDocumentation
    {

        Literal Literal { get; }

        StringBuilder GetDokuFor(Substitution sub);

    }
}
