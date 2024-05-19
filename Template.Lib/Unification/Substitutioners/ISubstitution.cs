using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification.Substitutioners
{
    public interface ISubstitution
    {
        Statement Apply(Statement statement);
        Literal Apply(Literal literal);

        Operation Apply(Operation operation);

        void Add(Term variable, AtomParam term);

        void Remove(Term variable);

        public IEnumerable<Mapping> Mappings { get; }

        public void BackPropagate(ISubstitution inductor);

        public ISubstitution Clone();

        public void Contract();
    }
}
