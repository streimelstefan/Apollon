using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;

namespace AppollonParser.Visitors
{
    internal class ProgramVisitor : apollonBaseVisitor<Program>
    {

        public override Program VisitProgram(apollonParser.ProgramContext context)
        {

            return new Program(new Literal[] { }, new Rule[] { });
        }

    }
}
