using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public interface IDocumentationGenerator
    {

        string GenerateDokumentationFor(Program program);

    }
}
