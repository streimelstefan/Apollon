using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public interface ISubstitution
    {
        Statement Apply(Statement statement);

        void Add(Term variable, AtomParam term);

        public IEnumerable<Mapping> Mappings { get; }
    }
}
