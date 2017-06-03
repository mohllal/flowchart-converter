using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class IfNode : WhileNode
    {
        private HolderNode middleNode;

        public HolderNode MiddleNode
        {
            get
            {
                return middleNode;
            }

            set
            {
                middleNode = value;
            }
        }

        public IfNode()
        {
            base.Name = "If";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            base.Shape.GradientColor = Color.Black;
            base.setText("IF");
        }

        public override void onShapeClicked()
        {
            //To be implemented
        }
    }
}
