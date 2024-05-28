using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.NMRCheck;

/// <summary>
/// A generator that generates NMRCheck rules for a given program.
/// </summary>
public interface INMRCheckGenerator
{
    /// <summary>
    /// Generates the NMRCheck rules for the given program.
    /// </summary>
    /// <param name="preprocessedStatements">The preprocessed statments that should be used to create the rules.</param>
    /// <param name="program">The program from which the statements originated from.</param>
    /// <returns>The check rules that were generated.</returns>
    Statement[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements, Program program);
}
