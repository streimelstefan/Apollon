//-----------------------------------------------------------------------
// <copyright file="CheckRule.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.NMRCheck;
using Apollon.Lib.Rules;

/// <summary>
/// Represents a rule that gets generated for the NMR check.
/// </summary>
public class CheckRule : Statement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckRule"/> class.
    /// A NMR Check rule.
    /// </summary>
    /// <param name="head">The head of the rule.</param>
    /// <param name="body">The body of the rule.</param>
    /// <exception cref="ArgumentNullException">Is thrown if the head of the rule is null.</exception>
    /// <exception cref="ArgumentException">Is thrown if the head of the rule is not NAF.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if the body contains no elements.</exception>
    public CheckRule(Literal head, params BodyPart[] body)
        : base(head, body)
    {
        if (this.Head == null)
        {
            throw new ArgumentNullException(nameof(head));
        }

        if (!this.Head.IsNAF)
        {
            throw new ArgumentException("Head of a NMR Check rule needs to be NAF.");
        }

        if (this.Body.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(this.Body), this.Body.Length, "Body needs to have at least one literal.");
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.Head} :- {string.Join(", ", this.Body!.Select(literal => literal?.ToString()))}.";
    }
}
