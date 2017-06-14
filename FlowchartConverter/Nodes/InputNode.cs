using Crainiate.Diagramming;
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
    class InputNode : BaseNode
    {
        public InputNode()
        {
            base.Name = "Input";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#0040a0");
            base.Shape.GradientColor = Color.Black;
            base.setText("Input");
        }

        protected override void showStatment()
        {
            base.setText("Read " + Statement);
        }

        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (base.Shape.Selected)
            {
                InputDialog db = new InputDialog();
                DialogResult dr = db.ShowDialog();

                if (dr == DialogResult.OK)
                    base.Statement = db.InputVariable;
            }
            base.Shape.Selected = false;
        }
    }
}
