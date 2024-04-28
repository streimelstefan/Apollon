using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Atoms
{
    public class Atom : IEquatable<Atom>, ICloneable
    {
        public string Name { get; set; }

        public AtomParam[] ParamList { get; set; }

        public Atom(string name, params AtomParam[] paramList)
        {
            Name = name;
            ParamList = paramList;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", ParamList.Select(term => term.ToString()))})";
        }


        public bool Equals(Atom? other)
        {
            if (other == null) return false;

            if (Name != other.Name || ParamList.Length != other.ParamList.Length)
            {
                return false;
            }

            // check if the term list is the same
            for (int i = 0; i < ParamList.Length; i++)
            {
                if (!ParamList[i].Equals(other.ParamList[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public object Clone()
        {
            return new Atom((string)Name.Clone(), new List<AtomParam>(ParamList.Select(p => (AtomParam)p.Clone())).ToArray());
        }
    }
}
