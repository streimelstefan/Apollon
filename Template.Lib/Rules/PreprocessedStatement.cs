//-----------------------------------------------------------------------
// <copyright file="PreprocessedStatement.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules
{
    /// <summary>
    /// A Statement that has its Metadata set.
    /// </summary>
    public class PreprocessedStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreprocessedStatement"/> class.
        /// </summary>
        /// <param name="statement">The Statement whose metadata should be set.</param>
        /// <param name="isOlonRule">Whether or not the statement is an OLON Rule.</param>
        /// <param name="isOrdiniaryRule">Whether or not the statement is an ordinary Rule.</param>
        public PreprocessedStatement(Statement statement, bool isOlonRule, bool isOrdiniaryRule)
            : base(statement.Head, statement.Body)
        {
            this.IsOlonRule = isOlonRule;
            this.IsOrdiniaryRule = isOrdiniaryRule;
        }

        /// <summary>
        /// Gets a value indicating whether or not the Rule is an OLON Rule.
        /// </summary>
        public bool IsOlonRule { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the Rule is an Ordinary Rule.
        /// </summary>
        public bool IsOrdiniaryRule { get; private set; }

        /// <summary>
        /// Clones the Preprocessed Statement.
        /// </summary>
        /// <returns>Returns a cloned PreprocessedStatement.</returns>
        public override object Clone()
        {
            return new PreprocessedStatement((Statement)base.Clone(), this.IsOlonRule, this.IsOrdiniaryRule);
        }

        /// <summary>
        /// Converts the Preprocessed Statement to a string.
        /// </summary>
        /// <returns>Returns a string containing the converted PreprocessedStatement.</returns>
        public override string ToString()
        {
            return $"{base.ToString()} IsOlon: {(this.IsOlonRule ? "True" : "False")} IsOrdinary: {(this.IsOrdiniaryRule ? "True" : "False")}";
        }
    }
}
