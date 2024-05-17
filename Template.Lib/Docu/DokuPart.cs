using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public class DokuPart
    {

        public string? DocuPart
        {
            get; private set;
        }

        public Term? VariablePlaceholder
        {
            get; private set;
        }

        public DokuPart(string docuPart)
        {
            DocuPart = docuPart;
        }

        public DokuPart(Term variable)
        {
            VariablePlaceholder = variable;
        }

    }
}
