using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.Rules
{
    public class Rule : IEquatable<Rule>
    {

        public Literal? Head { get; set; }
        public Literal[] Body { get; set; }

        public Rule(Literal? head, params Literal[] body)
        {
            if (head != null && head.IsNAF)
            {
                throw new ArgumentException("Head Literal is not allowed to be NAF negated.");
            }
            Head = head;
            Body = body;
        }

        public override string ToString()
        {
            return $"{Head} :- {string.Join(", ", Body.Select(literal => literal.ToString()))}";
        }

        public bool Equals(Rule? other)
        {
            if (other == null)
            {
                return false;
            }

            if (Head == null && Head != other.Head)
            {
                return false;
            }

            if (other.Head != null && Head != null && !other.Head.Equals(Head) || other.Body.Length != Body.Length)
            {
                return false;
            }

            // check if the term list is the same
            for (int i = 0; i < Body.Length; i++)
            {
                if (!Body[i].Equals(other.Body[i]))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
