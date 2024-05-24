using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Extensions
{
    public static class TermExtensions
    {

        public static bool IsNumber(this Term term)
        {
            return int.TryParse(term.Value, out _);
        }
    }
}
