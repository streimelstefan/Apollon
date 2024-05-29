namespace Apollon.Lib.Rules
{
    /// <summary>
    /// A Constraint is a Statement without a Head.
    /// </summary>
    public class Constraint : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="body">The body of the constraint.</param>
        public Constraint(params Literal[] body)
            : base(null, body.Select(literal => new BodyPart(literal, null)).ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="body">The body of the constraint.</param>
        public Constraint(params BodyPart[] body)
            : base(null, body)
        {
        }
    }
}
