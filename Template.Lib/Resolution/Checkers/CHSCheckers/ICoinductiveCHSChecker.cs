namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    using Apollon.Lib.Resolution.CallStackAndCHS;

    /// <summary>
    /// The CHS Checker checks the CHS for loops.
    /// </summary>
    public interface ICoinductiveCHSChecker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="literal"></param>
        /// <param name="chs"></param>
        /// <returns></returns>
        CheckerResult CheckCHSFor(Literal literal, CHS chs);
    }
}
