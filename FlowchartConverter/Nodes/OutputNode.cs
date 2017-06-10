using Crainiate.Diagramming.Flowcharting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class OutputNode : BaseNode
    {
        public OutputNode()
        {
            base.Name = "Output";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            base.Shape.StencilItem.BackColor = System.Drawing.Color.Green;
            base.Shape.StencilItem.GradientColor = Color.Black;
            base.setText("Output");
        }

        protected override void showStatment()
        {
            base.setText("Print " + Statement);
        }

        public override void onShapeClicked() { }
    }
}
