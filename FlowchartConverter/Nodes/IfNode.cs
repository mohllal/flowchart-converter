using Crainiate.Diagramming.Flowcharting;
using FlowchartConverter.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter.Nodes
{
    class IfNode : WhileNode
    {
        private HolderNode middleNode;

        public HolderNode MiddleNode
        {
            get
            {
                return middleNode;
            }

            set
            {
                middleNode = value;
            }
        }

        public IfNode()
        {
            base.Name = "If";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            base.Shape.GradientColor = Color.Black;
            base.setText("IF");
        }

        protected override void makeConnections()
        {
            this.MiddleNode = new HolderNode(this);
            this.MiddleNode.Shape.Label = new Crainiate.Diagramming.Label("Done");

            base.TrueNode = new HolderNode(this);
            base.TrueNode.Shape.Label = new Crainiate.Diagramming.Label("Start IF");
            base.TrueConnector = new ConnectorNode(this);
            base.TrueConnector.Connector.Opacity = 50;
            base.TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");

            base.BackNode = new HolderNode(this);
            base.BackNode.Shape.Label = new Crainiate.Diagramming.Label("End IF");
            base.BackNode.OutConnector.EndNode = this;
            base.BackNode.OutConnector.Connector.End.Shape = this.MiddleNode.Shape;
            base.BackNode.OutConnector.Connector.Opacity = 50;

            base.TrueNode.OutConnector.EndNode = base.BackNode;
            base.TrueConnector.Selectable = false;

            base.BackNode.OutConnector.Selectable = false;
            base.BackNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }

        protected override void moveConnections()
        {
            this.MiddleNode.NodeLocation = new PointF(base.Shape.Center.X - this.MiddleNode.Shape.Width / 2, 
                base.Shape.Center.Y - this.MiddleNode.Shape.Size.Height / 2);

            this.moveConnections();

            this.MiddleNode.NodeLocation = new PointF(this.MiddleNode.NodeLocation.X, base.BackNode.NodeLocation.Y);

            if (base.OutConnector.EndNode != null && base.OutConnector.EndNode.NodeLocation.Y < this.MiddleNode.NodeLocation.Y + base.shiftY)
                base.shiftMainTrack();
        }

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            clickedConnector.StartNode.attachNode(newNode);
            this.MiddleNode.NodeLocation = new PointF(this.MiddleNode.NodeLocation.X, base.BackNode.NodeLocation.Y);

            if (base.OutConnector.EndNode == null)
                return;

            if (base.OutConnector.EndNode.NodeLocation.Y < base.BackNode.NodeLocation.Y)
                shiftMainTrack();
        }

        public override void addToModel()
        {
            base.addToModel();
            this.MiddleNode.addToModel();
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
                IfDialog ifBox = new IfDialog();
                if (!String.IsNullOrEmpty(Statement))
                {
                    MessageBox.Show("Empty Statement!");
                }

                DialogResult dr = ifBox.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    base.Statement = ifBox.DecisionExpression;
                    base.setText(Statement);
                }
            }
            base.Shape.Selected = false;
        }

        private String surrondExpression(String str)
        {
            return "if ( " + str + " )";
        }

        private String extractExpression(String str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            String res = str.Remove(0, 5);
            res = res.Remove(res.Count() - 1);
            return res;
        }
    }
}
