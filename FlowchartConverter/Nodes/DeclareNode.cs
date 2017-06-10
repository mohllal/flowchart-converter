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
    class DeclareNode : BaseNode
    {
        public DeclareNode()
        {
            base.Name = "Declare";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.InternalStorage];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3810c");
            base.Shape.GradientColor = Color.Black;
            base.setText("Declare");
        }

        protected override void showStatment()
        {
            base.setText(Statement);
        }

        public override void onShapeClicked() {}
    }
}
