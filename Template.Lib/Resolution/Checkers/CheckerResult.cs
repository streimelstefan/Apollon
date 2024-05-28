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
