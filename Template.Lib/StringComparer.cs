//-----------------------------------------------------------------------
// <copyright file="StringComparer.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    /// <summary>
    /// A comparer that compares strings numerically if possible.
    /// </summary>
    public class StringComparer : IComparer<string>
    {
        /// <summary>
        /// Compares two strings numerically if possible, otherwise falls back to normal string comparison.
        /// </summary>
        /// <param name="x">String one that could be a number.</param>
        /// <param name="y">String two that could be a number.</param>
        /// <returns>A subtracted number if both strings were a number, otherwise a value representing the lexical relationship between two strings.</returns>
        public int Compare(string? x, string? y)
        {
            // Check if both strings are numeric
            if (int.TryParse(x, out int numX) && int.TryParse(y, out int numY))
            {
                // Compare numerically
                return numX - numY;
            }

            // Fall back to normal string comparison
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }
}
