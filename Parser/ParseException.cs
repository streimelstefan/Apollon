using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser
{
    /// <summary>
    /// An exception that gets thrown if there is an error within a parsed program that ocures during the parsing or lexing phase.
    /// </summary>
    public class ParseException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public ParseException(string message) : base(message)
        { }

    }
}
