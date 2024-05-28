using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.DualRules
{

    /// <summary>
    /// A class that switches the NAF of an <see cref="Operation"/>.
    /// </summary>
    public class OperationNAFSwitcher
    {

        private Dictionary<Operator, Operator> operatorSwitches;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationNAFSwitcher"/> class.
        /// </summary>
        public OperationNAFSwitcher() 
        {
            operatorSwitches = new Dictionary<Operator, Operator>();
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

            foreach (var mapping in this.operatorSwitches.ToArray())
            {
                this.operatorSwitches.Add(mapping.Value, mapping.Key);
            }
        }

    }
}
