//-----------------------------------------------------------------------
// <copyright file="CallStack.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.CallStackAndCHS
{
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// This class represents the CallStack of the Resolution algorithm. It saves all the literals that have been investigated until
    /// the currently investigated one.
    /// </summary>
    public class CallStack
    {
        private readonly IUnifier unifier = new Unifier();

        /// <summary>
        /// Initializes a new instance of the <see cref="CallStack"/> class.
        /// </summary>
        public CallStack()
        {
            this.Items = new List<CallStackItem>();
        }

        /// <summary>
        /// Gets the items of the CallStack in a List.
        /// </summary>
        public List<CallStackItem> Items { get; private set; } // List does preserve Order, as written on MSDN List<T> Class.

        /// <summary>
        /// Gets a value indicating whether the CallStack is empty.
        /// </summary>
        public bool IsEmpty => this.Items.Count == 0;

        /// <summary>
        /// Adds a new item to the CallStack.
        /// </summary>
        /// <param name="currentGoal">The current goal of the callstackitem.</param>
        /// <param name="applyingRules">A Queue with all applying Rules.</param>
        /// <param name="applyingSubstitution">The current applyingSubstitution.</param>
        /// <exception cref="ArgumentException">Is thrown when there is another literal in the chs that can be unified.</exception>
        public void Add(BodyPart currentGoal, Queue<Statement> applyingRules, Substitution applyingSubstitution)
        {
            // if there is another literal in the chs that can be unified.
            if (this.Items.Select(i => i.CurrentGoal).Where(l => this.unifier.Unify(l, currentGoal).IsSuccess).Any())
            {
                throw new ArgumentException("Literal already in CHS."); // Check is proffiecient, as shown in Tests.
            }

            this.Items.Add(new CallStackItem(currentGoal, applyingRules, applyingSubstitution));
        }

        /// <summary>
        /// Returns the last item of the CallStack without removing it.
        /// </summary>
        /// <returns>Returns the last item of the CallStack.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the CallStack is empty.</exception>
        public CallStackItem Peek()
        {
            return this.Items[^1] ?? throw new InvalidOperationException("Cannot Peek as CHS is empty!");
        }

        /// <summary>
        /// Returns the first item of the CallStack that has not finished yet.
        /// </summary>
        /// <returns>Returns the first item of the CallStack that has not finished yet without removing it.</returns>
        public CallStackItem PeekFirstNonFinished()
        {
            return this.Items.Where(i => i.ApplingRules.Count() > 0).Last();
        }

        /// <summary>
        /// Removes the last item of the CallStack and returns it.
        /// </summary>
        /// <returns>Returns the last item of the CallStack.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the CallStack is empty.</exception>
        public CallStackItem Pop()
        {
            CallStackItem item = this.Items[^1] ?? throw new InvalidOperationException("Cannot Pop as CHS is empty!");
            this.Items.RemoveAt(this.Items.Count - 1);
            return item;
        }

        /// <summary>
        /// Checks if the CallStack is empty.
        /// </summary>
        /// <returns>Returns a boolean representing whether the CallStack is empty.</returns>
        public bool Empty()
        {
            return this.Items.Count == 0;
        }

        /// <summary>
        /// Checks if there are no more rules to check.
        /// </summary>
        /// <returns>Returns a boolean representing whether there are more Rules to check.</returns>
        public bool NoMoreRulesToCheck()
        {
            return !this.Items.Where(i => i.ApplingRules.Count > 0).Any();
        }

        /// <summary>
        /// Returns a string representation of the CallStack.
        /// </summary>
        /// <returns>Returns a string representation of the entire CallStack.</returns>
        public override string ToString()
        {
            return $"{string.Join("\n", this.Items.Select(l => l.ToString()))}";
        }

        /// <summary>
        /// Converts the CallStack to a CHS.
        /// </summary>
        /// <returns>Returns a CHS with the same content as the CallStack.</returns>
        public CHS ConverToCHS()
        {
            // warning can be ignored literal in the goal needs has to be set.
            return new CHS(this.Items.Where(i => i.CurrentGoal.Literal != null).Select(i => i.ApplingSubstitution.Apply(i.CurrentGoal.Literal!)));
        }

        /// <summary>
        /// Converts the CallStack to a CHS without the last item.
        /// </summary>
        /// <returns>Returns a CHS with the same content as the CallStack but without the last item.</returns>
        public CHS ConvertToCHSWithoutLast()
        {
            // warning can be ignored literal in the goal needs has to be set.
            CallStackItem last = this.Items.Last();
            return new CHS(this.Items.Where(i => i.CurrentGoal.Literal != null).TakeWhile(i => i != last).Select(i => i.ApplingSubstitution.Apply(i.CurrentGoal.Literal!)));
        }

        /// <summary>
        /// This class represents an item in the CallStack.
        /// </summary>
        public class CallStackItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CallStackItem"/> class.
            /// </summary>
            /// <param name="goal">The current goal of the callstackitem.</param>
            /// <param name="applyingRules">A Queue with all applying Rules.</param>
            /// <param name="applyingSubstitution">The current applyingSubstitution.</param>
            public CallStackItem(BodyPart goal, Queue<Statement> applyingRules, Substitution applyingSubstitution)
            {
                this.CurrentGoal = goal;
                this.ApplingRules = applyingRules;
                this.ApplingSubstitution = applyingSubstitution;
            }

            /// <summary>
            /// Gets the current goal of the CallStackItem.
            /// </summary>
            public BodyPart CurrentGoal { get; private set; }

            /// <summary>
            /// Gets the Queue of applying Rules.
            /// </summary>
            public Queue<Statement> ApplingRules { get; private set; }

            /// <summary>
            /// Gets the current applying Substitution.
            /// </summary>
            public Substitution ApplingSubstitution { get; private set; }

            /// <summary>
            /// Converts the CallStackItem to a string.
            /// </summary>
            /// <returns>Returns a string representation of the CallStackItem.</returns>
            public override string ToString()
            {
                return $"{this.CurrentGoal} ({this.ApplingSubstitution}), T = [{string.Join(", ", this.ApplingRules.Select(rule => rule.ToString()))}]!";
            }
        }
    }
}
