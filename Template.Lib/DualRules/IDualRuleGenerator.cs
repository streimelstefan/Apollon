using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.DualRules
{
    public interface IDualRuleGenerator
    {

        DualRule[] GenerateDualRules(Statement[] statements);

    }
}
