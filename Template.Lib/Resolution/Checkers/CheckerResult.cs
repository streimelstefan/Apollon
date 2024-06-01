//-----------------------------------------------------------------------
// <copyright file="CheckerResult.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers
{
    /// <summary>
    /// The Result of a Check.
    /// </summary>
    public enum CheckerResult
    {
        /// <summary>
        /// The Check Succeeds.
        /// </summary>
        Succeed,

        /// <summary>
        /// The Check Fails.
        /// </summary>
        Fail,

        /// <summary>
        /// The Check needs to be continued.
        /// </summary>
        Continue,
    }
}
