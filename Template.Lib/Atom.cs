using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib
{
    public class Atom : IEquatable<Atom>
    {
        public string Name { get; set; }

        public Term[] TermList { get; set; }

        public Atom(string name, params Term[] termList)
        {
            Name = name;
            TermList = termList;
        }

        public override string ToString()
        {
            return $"{Name}({String.Join(", ", TermList.Select(term => term.ToString()))})";
        }


        public bool Equals(Atom other)
        {
            if (other == null) return false;

            if (Name != other.Name || TermList.Length != other.TermList.Length)
            {
                return false;
            }

            // check if the term list is the same
            for (int i = 0; i < TermList.Length; i++) 
            {
                if (!TermList[i].Equals(other.TermList[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
