using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.DualRules
{
    public class OperationNAFSwitcher
    {

        private Dictionary<Operator, Operator> operatorSwitches;

        public OperationNAFSwitcher() 
        {
            operatorSwitches = new Dictionary<Operator, Operator>();
            this.PopulateOperatorSwitches();
        }

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
