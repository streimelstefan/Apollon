//-----------------------------------------------------------------------
// <copyright file="OperationNAFSwitcher.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.DualRules
{
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// A class that switches the NAF of an <see cref="Operation"/>.
    /// </summary>
    public class OperationNAFSwitcher
    {
        private readonly Dictionary<Operator, Operator> operatorSwitches;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationNAFSwitcher"/> class.
        /// </summary>
        public OperationNAFSwitcher()
        {
            this.operatorSwitches = new Dictionary<Operator, Operator>();
            this.PopulateOperatorSwitches();
        }

        /// <summary>
        /// Switches the NAF of the given <see cref="Operation"/>.
        /// </summary>
        /// <param name="operation">The operation where the NAF was switched.</param>
        public void SwitchNaf(Operation operation)
        {
            if (operation.OutputtingVariable != null)
            {
                operation.IsNAF = !operation.IsNAF;
            }
            else
            {
                operation.Operator = this.operatorSwitches[operation.Operator];
            }
        }

        private void PopulateOperatorSwitches()
        {
            this.operatorSwitches.Clear();
            this.operatorSwitches.Add(Operator.Equals, Operator.NotEquals);
            this.operatorSwitches.Add(Operator.LessThan, Operator.GreaterThanOrEqual);
            this.operatorSwitches.Add(Operator.GreaterThan, Operator.LessThanOrEqual);

            foreach (KeyValuePair<Operator, Operator> mapping in this.operatorSwitches.ToArray())
            {
                this.operatorSwitches.Add(mapping.Value, mapping.Key);
            }
        }
    }
}
