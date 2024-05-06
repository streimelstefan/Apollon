using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class NotSubstitutableException : Exception
    {

        public NotSubstitutableException() : base("Value is not substitutable")
        { }

    }
}
