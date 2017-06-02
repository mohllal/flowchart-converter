using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class TerminalNode : BaseNode
    {
        public enum TerminalType { Start, End }

        public TerminalNode(TerminalType termType)
        {
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Terminator];
            base.Shape.BackColor = Color.Magenta;
            base.Shape.StencilItem.GradientColor = Color.Black;

            if (termType == TerminalType.Start) {
                base.Statement = "Start";
                base.setText("Start");
                base.Name = "Start";
                base.NodeLocation = new PointF(80, 10);
                base.ParentNode = this;
            }
            else {
                base.Statement = "End";
                base.setText("End");
                base.Name = "End";
            }
        }

        public override void onShapeClicked() { }
    }
}
