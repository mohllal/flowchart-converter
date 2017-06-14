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

        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (base.Shape.Selected)
            {
                AssignDialog db = new AssignDialog();
                DialogResult dr = db.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(db.AssignmentVariable) || String.IsNullOrEmpty(db.AssignmentExpression)
                        || String.IsNullOrWhiteSpace(db.AssignmentVariable) || String.IsNullOrWhiteSpace(db.AssignmentExpression))
                    {
                        MessageBox.Show("You must enter a valid assignment expression");
                        return;
                    }
                    base.Statement = db.AssignmentVariable + " = " + db.AssignmentExpression;
                }
            }
            base.Shape.Selected = false;
        }
    }
}
