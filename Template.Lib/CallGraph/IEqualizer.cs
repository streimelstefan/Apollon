using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Graph
{
    public interface IEqualizer<T>
    {

        bool AreEqual(T first, T second);

    }
}
