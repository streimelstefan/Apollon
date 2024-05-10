using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CallStackAndCHS
{
    public class CallStack
    {
        public List<CallStackItem> Items { get; private set; } //List does preserve Order, as written on MSDN List<T> Class.
        private IUnifier Unifier = new Unifier();

        public bool IsEmpty
        {
            get
            {
                return Items.Count == 0;
            }
        }

        public CallStack()
        {
            Items = new List<CallStackItem>();
        }

        public void Add(BodyPart currentGoal, Queue<Statement> applingRules, ISubstitution applyingSubstitution)
        {
            if (Items.Select(i => i.CurrentGoal).Where(l => Unifier.Unify(l, currentGoal).IsSuccess).Any()) // if there is another literal in the chs that can be unified.
            {
                throw new ArgumentException("Literal already in CHS."); // Check is proffiecient, as shown in Tests.
            }

            Items.Add(new CallStackItem(currentGoal, applingRules, applyingSubstitution));
        }


        public CallStackItem Peek()
        {
            return Items[Items.Count - 1] ?? throw new InvalidOperationException("Cannot Peek as CHS is empty!");
        }

        public CallStackItem PeekFirstNonFinished()
        {
            return Items.Where(i => i.ApplingRules.Count() > 0).Last();
        }

        public CallStackItem Pop()
        {
            var item = Items[Items.Count - 1] ?? throw new InvalidOperationException("Cannot Pop as CHS is empty!");
            Items.RemoveAt(Items.Count - 1);
            return item;
        }

        public bool Empty()
        {
            return Items.Count == 0;
        }

        public bool NoMoreRulesToCheck()
        {
            return !Items.Where(i => i.ApplingRules.Count > 0).Any();
        }

        public override string ToString()
        {
            return $"{string.Join("\n", Items.Select(l => l.ToString()))}";
        }

        public CHS ConverToCHS()
        {
            // warning can be ignored literal in the goal needs has to be set.
            return new CHS(Items.Where(i => i.CurrentGoal.Literal != null).Select(i => i.ApplingSubstitution.Apply(i.CurrentGoal.Literal)));
        }

        public CHS ConvertToCHSWithoutLast()
        {
            // warning can be ignored literal in the goal needs has to be set.
            var last = Items.Last();
            return new CHS(Items.Where(i => i.CurrentGoal.Literal != null).TakeWhile(i => i != last).Select(i => i.ApplingSubstitution.Apply(i.CurrentGoal.Literal)));
        }

        public class CallStackItem
        {
            public BodyPart CurrentGoal { get; private set; }
            public Queue<Statement> ApplingRules { get; private set; }

            public ISubstitution ApplingSubstitution { get; private set; }

            public CallStackItem(BodyPart goal, Queue<Statement> applingRules, ISubstitution applingSubstitution)
            {
                CurrentGoal = goal;
                ApplingRules = applingRules;
                ApplingSubstitution = applingSubstitution;
            }

            public override string ToString()
            {
                return $"{CurrentGoal} ({ApplingSubstitution}), T = [{string.Join(", ", ApplingRules.Select(rule => rule.ToString()))}]!";
            }
        }
    }
}
