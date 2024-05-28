using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Term"/>.
    /// </summary>
    public static class TermExtensions
    {
        /// <summary>
        /// Checks if the term is a number.
        /// </summary>
        /// <param name="term">The term to check.</param>
        /// <returns>If the term can be casted into an <see cref="int"/>.</returns>
        public static bool IsNumber(this Term term)
        {
            return int.TryParse(term.Value, out _);
        }
    }
}
