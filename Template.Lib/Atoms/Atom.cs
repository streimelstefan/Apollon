//-----------------------------------------------------------------------
// <copyright file="Atom.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Atoms
{
    /// <summary>
    /// An atom is a structure that looks something like this: a(X), a(a(X)), a(X, Y), a(a).
    /// </summary>
    public class Atom : IEquatable<Atom>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Atom"/> class.
        /// </summary>
        /// <param name="name">The name of the atom.</param>
        /// <param name="paramList">The parameters of the atom.</param>
        public Atom(string name, params AtomParam[] paramList)
        {
            this.Name = name;
            this.ParamList = paramList;
        }

        /// <summary>
        /// Gets or sets the name of the atom.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters of the atom.
        /// </summary>
        public AtomParam[] ParamList { get; set; }

        /// <summary>
        /// Generates the string representation of the current object.
        /// </summary>
        /// <returns>The string representation of the object.</returns>
        public override string ToString()
        {
            return $"{this.Name}({string.Join(", ", this.ParamList.Select(term => term.ToString()))})";
        }

        /// <summary>
        /// Checks whether the other atom is the equal to the current one.
        /// </summary>
        /// <param name="other">The other atom to compare to.</param>
        /// <returns>Whether the atoms are equal or not.</returns>
        public bool Equals(Atom? other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Name != other.Name || this.ParamList.Length != other.ParamList.Length)
            {
                return false;
            }

            // check if the term list is the same
            for (int i = 0; i < this.ParamList.Length; i++)
            {
                if (!this.ParamList[i].Equals(other.ParamList[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clones the current object.
        /// </summary>
        /// <returns>The clone of the current object.</returns>
        public object Clone()
        {
            return new Atom((string)this.Name.Clone(), new List<AtomParam>(this.ParamList.Select(p => (AtomParam)p.Clone())).ToArray());
        }
    }
}
