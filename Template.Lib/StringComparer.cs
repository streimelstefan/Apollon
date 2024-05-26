using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib
{
    internal class StringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
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
