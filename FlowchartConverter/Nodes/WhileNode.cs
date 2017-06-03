using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class WhileNode : DecisionNode
    {
        public WhileNode()
        {
            base.Name = "While";
            base.setText("While");
        }

        public override void onShapeClicked()
        {
            //To be implemented 
        }
    }
}
