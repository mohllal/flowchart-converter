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
        public class Variable
        {
            public enum Data_Type { INTEGER, STRING, REAL, BOOLEAN, CHAR }

            private String varName;
            private Data_Type varType;
            private bool single;
            private int size;

            public string VarName
            {
                get
                {
                    return varName;
                }

                set
                {
                    varName = value;
                }
            }

            public Data_Type VarType
            {
                get
                {
                    return varType;
                }

                set
                {
                    varType = value;
                }
            }

            public bool Single
            {
                get
                {
                    return single;
                }

                set
                {
                    single = value;
                }
            }

            public int Size
            {
                get
                {
                    return size;
                }

                set
                {
                    size = value;
                }
            }
        }

        private Variable variable;

        public Variable _Var
        {
            get
            {
                return variable;
            }

            set
            {
                variable = value;
            }
        }

        public DeclareNode()
        {
            base.Name = "Declare";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.InternalStorage];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3810c");
            base.Shape.GradientColor = Color.Black;
            base.setText("Declare");

            this.variable = new Variable();
        }

        protected override void showStatment()
        {
            base.setText(base.Statement);
        }

        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {
                TextBox textBox = new TextBox();
                textBox.Location = new Point((int)Shape.Location.X, (int)Shape.Location.Y);
                textBox.Width = (int)Shape.Width;
                DeclareDialog db = new DeclareDialog();
                DialogResult dr = db.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(db.DeclareDataType) || String.IsNullOrEmpty(db.DeclareVariableType)
                        || String.IsNullOrWhiteSpace(db.DeclareDataType) || String.IsNullOrWhiteSpace(db.DeclareVariableType)
                        || String.IsNullOrEmpty(db.DeclareVariable) || String.IsNullOrWhiteSpace(db.DeclareVariable))
                    {
                        MessageBox.Show("You must enter a valid declare expression");
                        return;
                    }

                    if (db.DeclareVariableType.Equals("Array") && (String.IsNullOrEmpty(db.DeclareArraySize) || String.IsNullOrWhiteSpace(db.DeclareArraySize)))
                    {
                        MessageBox.Show("You must enter a valid declare expression");
                        return;
                    }
                    this.initializeVariable(db);
                    this.makeStatment();
                }
            }
            base.Shape.Selected = false;
        }

        private void makeStatment()
        {
            if (this.variable.Single)
                base.Statement = this.variable.VarType + "  " + this.variable.VarName;
            else
                base.Statement = this.variable.VarType + " Array " + this.variable.VarName + "[" + this.variable.Size + "]";

        }

        private void initializeVariable(DeclareDialog db)
        {
            this.variable.VarName = db.DeclareVariable;
            if (db.DeclareDataType.Equals("Integer"))
                this.variable.VarType = Variable.Data_Type.INTEGER;
            else if (db.DeclareDataType.Equals("Float"))
                this.variable.VarType = Variable.Data_Type.REAL;
            else if (db.DeclareDataType.Equals("Bool"))
                this.variable.VarType = Variable.Data_Type.BOOLEAN;
            else
                this.variable.VarType = Variable.Data_Type.STRING;


            if (db.DeclareVariableType.Equals("Array"))
            {
                this.variable.Single = false;

                this.variable.Size = Int32.Parse(db.DeclareArraySize);
            }
            else
                this.variable.Single = true;
        }
    }
}
