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
    class ForNode : WhileNode
    {
        private String var;
        private String startVal;
        private String endVal;
        private String stepBy;
        private String direction;

        public string Var
        {
            get
            {
                return var;
            }

            set
            {
                var = value;
            }
        }

        public string StartVal
        {
            get
            {
                return startVal;
            }

            set
            {
                startVal = value;
            }
        }

        public string EndVal
        {
            get
            {
                return endVal;
            }

            set
            {
                endVal = value;
            }
        }

        public string StepBy
        {
            get
            {
                return stepBy;
            }

            set
            {
                stepBy = value;
            }
        }

        public string Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        public ForNode()
        {
            base.Name = "For";
            base.setText("For");
            base.TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("Next");
            base.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }

        public override void onShapeClicked()
        {
            if (base.Shape.Selected && Controller.DeleteChoosed)
            {
                base.removeFromModel();
                Controller.DeleteChoosed = false;
                base.Shape.Selected = false;
            }

            if (base.Shape.Selected)
            {
                ForDialog forBox = new ForDialog();
                if (forBox.ShowDialog() == DialogResult.OK)
                {

                    this.setExpVariables(forBox);
                    base.Statement = generateEx();
                }
            }
            base.Shape.Selected = false;
        }

        protected override void showStatment()
        {
            base.setText(this.Var + " = " + this.StartVal + " to " + this.EndVal + " " + this.Direction);
        }

        private string generateEx()
        {
            return "int " + Var + " = "
                + StartVal + " ; " + Var + getDirectionCondition()
                + " " + EndVal + " ; " + Var + getDirectionOperator()
                + StepBy;
        }

        private void setExpVariables(ForDialog forBox)
        {
            Var = forBox.LoopVariable;
            StartVal = forBox.LoopStart;
            EndVal = forBox.LoopEnd;
            StepBy = forBox.LoopStep;
            Direction = forBox.LoopBehaviour;
        }

        private string surrondExpression(string str)
        {
            return "for ( " + str + " )";
        }

        private string getDirectionOperator()
        {
            if (Direction.Equals("Increment"))
                return "+=";
            return "-=";
        }

        private string getDirectionCondition()
        {
            if (Direction.Equals("Increment"))
                return "<";
            return ">";
        }
    }
}
