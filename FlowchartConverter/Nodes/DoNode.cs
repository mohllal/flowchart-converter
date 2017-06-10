using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class DoNode : DecisionNode
    {

        private HolderNode startNode;

        public DoNode() {
            base.Name = "DoWhile";
            base.setText("Do While");
        }
    
        protected override void makeConnections()
        {
            this.startNode = new HolderNode(this);
            base.TrueConnector = new ConnectorNode(this);
            base.TrueConnector.Connector.Opacity = 50;
            base.TrueNode = new HolderNode(this);
            base.TrueConnector.EndNode = this.startNode;

            this.startNode.OutConnector.EndNode = base.TrueNode;
            base.TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            base.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");

            base.BackNode = new HolderNode(this);
            base.BackNode.OutConnector.Connector.End.Shape = base.Shape;
            base.BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");

            base.TrueNode.OutConnector.EndNode = base.BackNode;

            base.BackNode.OutConnector.setEndNode(this);
            base.BackNode.OutConnector.Selectable = false;
            base.TrueConnector.Selectable = false;
            this.startNode.OutConnector.Selectable = false;

            base.BackNode.OutConnector.Connector.Opacity = 50;
            this.startNode.OutConnector.Connector.Opacity = 50;
            base.TrueConnector.Connector.Opacity = 50;   
        }

        protected override void moveConnections()
        {
            base.Shape.Location = new PointF(base.nodeLocation.X, base.nodeLocation.Y + base.shiftY);
            PointF point = new PointF(base.Shape.Width + base.Shape.Location.X + horizontalSpace, this.startNode.Shape.Center.Y - base.TrueNode.Shape.Size.Height / 2);
            PointF oldPlace = base.TrueNode.NodeLocation;
            base.TrueNode.NodeLocation = point;

            if (base.TrueConnector.EndNode == null)
            {
                base.TrueConnector.EndNode = TrueNode;
                base.TrueNode.attachNode(BackNode);
                return;
            }

            base.BackNode.NodeLocation = new PointF(point.X, base.BackNode.NodeLocation.Y);

            if (base.moveDirection == MOVE_DOWN)
                base.TrueNode.OutConnector.EndNode.shiftDown(base.moreShift);

            else if (base.moveDirection == MOVE_UP)
            {
                base.TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point.Y);
            }
        }

        public override Shape connectedShape()
        {
            return this.startNode.Shape;
        }

        public override void addToModel()
        {
            Controller.Model.Shapes.Add(base.Shape);

            Controller.Model.Shapes.Add(this.startNode.Shape);
            Controller.Model.Lines.Add(base.BackNode.OutConnector.Connector);
            Controller.Model.Lines.Add(this.startNode.OutConnector.Connector);
            base.addToModel();
        }

        public override void shiftMainTrack(int moreShift=0)
        {
            base.Shape.Location = new PointF(base.Shape.Location.X, base.BackNode.Shape.Center.Y- base.Shape.Size.Height / 2);
            if (base.OutConnector.EndNode != null)
                base.OutConnector.EndNode.shiftDown(base.moreShift);   
        }

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector) {
            
            if (clickedConnector == base.OutConnector)
                base.attachNode(newNode, clickedConnector);

            else if (clickedConnector.StartNode == BackNode)
                base.attachToTrue(newNode, true);

            else if (clickedConnector.StartNode == startNode )
                base.attachToTrue(newNode, false);

            else
                clickedConnector.StartNode.attachNode(newNode);

            this.shiftMainTrack();
        }

        public override void onShapeClicked() {}
    }

}
