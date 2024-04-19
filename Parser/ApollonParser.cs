using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Template.Lib;
using AppollonParser.Visitors;

namespace AppollonParser
{
    public class ApollonParser
    {

        public Program ParseFromString(string programString)
        {
            return this.ParseFromStream(CharStreams.fromString(programString));
        }

        private Program ParseFromStream(ICharStream stream)
        {
            var lexer = new apollonLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new apollonParser(tokens);
            var tree = parser.program();
            
            return tree.Accept(new ProgramVisitor());
        }

    }
}
