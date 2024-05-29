namespace Apollon.Lib.Rules
{
    /// <summary>
    /// A Rule is a Statement with a Head and a Body.
    /// </summary>
    public class Rule : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="head">The head of the Rule.</param>
        /// <param name="body">The body of the Rule.</param>
        /// <exception cref="ArgumentNullException">Is thrown when the head is null.</exception>
        /// <exception cref="ArgumentException">Is thrown when the head is NAF negated.</exception>
        public Rule(Literal head, params BodyPart[] body)
            : base(head, body)
        {
            if (this.Head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            if (this.Head.IsNAF)
            {
                throw new ArgumentException("Head Literal is not allowed to be NAF negated.");
            }
        }
    }
}
