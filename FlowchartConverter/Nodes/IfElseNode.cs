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
    class IfElseNode : IfNode
    {
        private HolderNode falseNode;
        private HolderNode backfalseNode;
        private ConnectorNode falseConnector;

        private bool moveFalsePart = true;

        public HolderNode FalseNode
        {
            get
            {
                return falseNode;
            }

            set
            {
                falseNode = value;
            }
        }

        public HolderNode BackfalseNode
        {
            get
            {
                return backfalseNode;
            }

            set
            {
                backfalseNode = value;
            }
        }

        public ConnectorNode FalseConnector
        {
            get
            {
                return falseConnector;
            }

            set
            {
                falseConnector = value;
            }
        }

        public bool MoveFalsePart
        {
            get
            {
                return moveFalsePart;
            }

            set
            {
                moveFalsePart = value;
            }
        }

        public IfElseNode()
        {
            base.Name = "IfElse";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            base.Shape.GradientColor = Color.Black;
            base.setText("IF");
        }

        public override void addRemoveFlag(bool v)
        {
            base.addRemoveFlag(v);

            this.BackfalseNode.ToBeRemoved = true;
            this.FalseNode.ToBeRemoved = true;

            BaseNode nextNode = FalseNode;
            while (nextNode.OutConnector.EndNode != this.BackfalseNode)
            {
                nextNode.OutConnector.EndNode.addRemoveFlag(true);
                nextNode = nextNode.OutConnector.EndNode;
            }
            nextNode = null;
            base.ToBeRemoved = true;
        }

        public bool isEmptyElse()
        {
            return this.FalseNode.OutConnector.EndNode == this.BackfalseNode;
        }

        protected override void makeConnections()
        {
            base.makeConnections();
            this.OutConnector = MiddleNode.OutConnector;

            this.FalseNode = new HolderNode(this);
            this.FalseNode.Shape.Label = new Crainiate.Diagramming.Label("Start Else");
            this.FalseConnector = new ConnectorNode(this);
            this.FalseConnector.Connector.Opacity = 50;
            this.FalseConnector.Connector.Label = new Crainiate.Diagramming.Label("False");

            this.BackfalseNode = new HolderNode(this);
            this.BackfalseNode.Shape.Label = new Crainiate.Diagramming.Label("End Else");
            this.BackfalseNode.OutConnector.EndNode = this;
            this.BackfalseNode.OutConnector.Connector.End.Shape = base.MiddleNode.Shape;
            this.BackfalseNode.OutConnector.Connector.Opacity = 50;

            this.FalseNode.OutConnector.EndNode = this.BackfalseNode;

            this.FalseConnector.Selectable = false;

            this.BackfalseNode.OutConnector.Selectable = false;
            this.BackfalseNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }

        protected override void moveConnections()
        {
            base.moveConnections();

            if (!this.MoveFalsePart)
                return;

            PointF point2 = new PointF(base.Shape.Location.X - horizontalSpace, base.TrueNode.NodeLocation.Y);
            PointF oldPlace = this.FalseNode.NodeLocation;
            this.FalseNode.NodeLocation = point2;

            if (this.FalseConnector.EndNode == null)
            {
                this.FalseConnector.EndNode = this.FalseNode;
                this.FalseNode.OutConnector.EndNode = this.BackfalseNode;
            }

            if (this.FalseNode.OutConnector.EndNode is HolderNode)
            {
                if (base.moveDirection == MOVE_UP)
                    this.FalseNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point2.Y);
                else
                    this.BackfalseNode.NodeLocation = new PointF(point2.X, point2.Y + 60);
            }

            else
            {
                this.BackfalseNode.NodeLocation = new PointF(point2.X, this.BackfalseNode.NodeLocation.Y);

                if (base.moveDirection == MOVE_DOWN)
                    this.FalseNode.OutConnector.EndNode.shiftDown(base.moreShift);

                else if (base.moveDirection == MOVE_UP)
                    this.FalseNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point2.Y);
            }
            this.balanceHolderNodes();
        }

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            ConnectorNode temp = clickedConnector;

            while (temp.EndNode is HolderNode)
                temp = temp.EndNode.OutConnector;

            clickedConnector.StartNode.attachNode(newNode);

            if (base.OutConnector.EndNode == null)
                return;

            if (base.OutConnector.EndNode.NodeLocation.Y < this.BackNode.NodeLocation.Y ||
                base.OutConnector.EndNode.NodeLocation.Y < this.BackfalseNode.NodeLocation.Y)
                base.shiftMainTrack();

            this.balanceHolderNodes();
        }

        public void balanceHolderNodes()
        {
            float y = base.MiddleNode.NodeLocation.Y;

            base.MiddleNode.NodeLocation = new PointF(base.MiddleNode.NodeLocation.X,
                (this.BackfalseNode.NodeLocation.Y > this.BackNode.NodeLocation.Y) ?
                this.BackfalseNode.NodeLocation.Y : this.BackNode.NodeLocation.Y);

            if (base.OutConnector.EndNode != null && base.OutConnector.EndNode.NodeLocation.Y < base.MiddleNode.NodeLocation.Y + base.shiftY)
                base.shiftMainTrack();
        }

        public override void addToModel()
        {
            base.addToModel();
            this.FalseNode.addToModel();
            this.BackfalseNode.addToModel();
        }

        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (base.Shape.Selected)
            {

                IfDialog ifBox = new IfDialog();
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
