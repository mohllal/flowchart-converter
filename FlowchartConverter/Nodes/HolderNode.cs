using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class HolderNode : BaseNode
    {
        private static int hCounter = 1;

        public HolderNode(BaseNode parentNode)
        {
            base.ParentNode = parentNode;
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Connector];
            base.Shape.MinimumSize = new System.Drawing.SizeF(15, 15);
            base.Shape.BackColor = System.Drawing.Color.White;
            base.Shape.Size = new System.Drawing.SizeF(15, 15);
            base.Shape.GradientColor = System.Drawing.Color.White;
            base.ShapeTag = "Shape_Holder" + hCounter.ToString();
            hCounter++;
        }
    }
}
