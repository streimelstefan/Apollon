//-----------------------------------------------------------------------
// <copyright file="TermExtensions.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

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
