using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    /// <summary>
    /// A part of a documentation. It can be either a string or a variable placeholder.
    /// </summary>
    public class DokuPart
    {

        /// <summary>
        /// Gets the string representation of the documentation part. Is only set if the part is a string.
        /// </summary>
        public string? DocuPart
        {
            get; private set;
        }

        /// <summary>
        /// Gets the variable placeholder of the documentation part. Is only set if the part is a variable placeholder.
        /// </summary>
        public Term? VariablePlaceholder
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DokuPart"/> class. The part is a string.
        /// </summary>
        /// <param name="docuPart">The string should be part of a documentation.</param>
        public DokuPart(string docuPart)
        {
            DocuPart = docuPart;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DokuPart"/> class. This part is a variable placeholder.
        /// </summary>
        /// <param name="variable">The variable that should be replaced with an value when the documentation gets built.</param>
        public DokuPart(Term variable)
        {
            VariablePlaceholder = variable;
        }
    }
}
