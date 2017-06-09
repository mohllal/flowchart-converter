using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public override void shiftDown(float moreShift)
        {
            base.NodeLocation = new PointF(base.NodeLocation.X, base.NodeLocation.Y + base.shiftY + base.moreShift);
            if (base.ParentNode is IfElseNode)
                (base.ParentNode as IfElseNode).balanceHolderNodes();

            else if (base.ParentNode.OutConnector.EndNode.NodeLocation.Y < base.NodeLocation.Y + base.shiftY)
                ((DecisionNode)base.ParentNode).shiftMainTrack();
        }

        public override void onShapeClicked() { }
    }
}