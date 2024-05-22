using Apollon.Lib.Logging;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public interface IResolution
    {

        IEnumerable<ResolutionResult> Resolute(Statement[] statements, BodyPart[] goals, ILogger logger);

    }
}
