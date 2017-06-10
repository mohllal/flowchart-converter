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
    class AssignNode : BaseNode
    {       
        public AssignNode()
        {
            base.Name = "Assign";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Default];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#fdfd80");
            base.Shape.GradientColor = Color.Black;
            base.setText("Assign");
        }

        public override void onShapeClicked() { }
    }
}
