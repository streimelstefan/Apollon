//-----------------------------------------------------------------------
// <copyright file="Operator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules.Operations
{
    /// <summary>
    /// An Enum that represents the different Operators that can be used in a Operation.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Indicates that one value is equal to another.
        /// </summary>
        Equals,

        /// <summary>
        /// Indicates that one value is not equal to another.
        /// </summary>
        NotEquals,

        /// <summary>
        /// Indicates that one value is greater than another.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Indicates that one value is greater than or equal to another.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Indicates that one value is less than another.
        /// </summary>
        LessThan,

        /// <summary>
        /// Indicates that one value is less than or equal to another.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Indicates that one value added to another.
        /// </summary>
        Plus,

        /// <summary>
        /// Indicates that one value subtracted from another.
        /// </summary>
        Minus,

        /// <summary>
        /// Indicates that one value is multiplied by another.
        /// </summary>
        Times,

        /// <summary>
        /// Indicates that one value is divided by another.
        /// </summary>
        Divide,
    }
}
