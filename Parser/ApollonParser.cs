//-----------------------------------------------------------------------
// <copyright file="ApollonParser.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser
{
    using Antlr4.Runtime;
    using Apollon.Lib;
    using Apollon.Lib.Rules;
    using AppollonParser.Visitors;

    /// <summary>
    /// The high level class that handles all the logic of parsing an apollo program.
    /// </summary>
    public class ApollonParser
    {
        /// <summary>
        /// Parses a program from a normal string.
        /// </summary>
        /// <param name="programString">The string to parse.</param>
        /// <returns>The parsed program.</returns>
        public Program ParseFromString(string programString)
        {
            return this.ParseFromStream(CharStreams.fromString(programString));
        }

        /// <summary>
        /// Parses a program from a file. The string given needs to point to a valid file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The parsed program.</returns>
        /// <exception cref="ArgumentException">Is thrown if the filepath is not valid.</exception>
        public Program ParseFromFile(string filePath)
        {
            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(filePath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Given path is not valid", nameof(filePath), e);
            }

            string code = File.ReadAllText(fullPath);

            return this.ParseFromString(code);
        }

        private Program ParseFromStream(ICharStream stream)
        {
            apollonLexer lexer = new(stream);
            CommonTokenStream tokens = new(lexer);
            apollonParser parser = new(tokens);
            apollonParser.ProgramContext tree = parser.program();

            return tree.Accept(new ProgramVisitor());
        }

        /// <summary>
        /// Parses a query from a string.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The parsed query.</returns>
        public BodyPart[] ParseQueryFromString(string query)
        {
            return this.ParseQueryFromStream(CharStreams.fromString(query));
        }

        private BodyPart[] ParseQueryFromStream(ICharStream stream)
        {
            apollonLexer lexer = new(stream);
            CommonTokenStream tokens = new(lexer);
            apollonParser parser = new(tokens);
            apollonParser.QueryContext tree = parser.query();

            return tree.Accept(new QueryVisitor());
        }
    }
}
