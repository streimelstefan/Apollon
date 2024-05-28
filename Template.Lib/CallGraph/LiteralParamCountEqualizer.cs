using Apollon.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Graph
{
    /// <summary>
    /// Checks the equality of two literals based on their name and the lenght of their term list.
    ///
    /// Using this equalizer following Literals would be equal:
    /// a(1, X).
    /// a(fds, asdfs).
    /// a(x, 3).
    ///
    /// While these would not be equal:
    /// a(1, X).
    /// a(1, 2, 3).
    /// b(1, 2).
    /// </summary>
    public class LiteralParamCountEqualizer : IEqualizer<Literal>
    {

        /// <summary>
        /// Compares two objects of type T and returns whether they are equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns>Whether or not the objects are equal.</returns>
        public bool AreEqual(Literal first, Literal second)
        {
            return first.Atom.Name == second.Atom.Name && 
                first.Atom.ParamList.Length == second.Atom.ParamList.Length &&
                first.IsNegative == second.IsNegative;
        }
    }
}
