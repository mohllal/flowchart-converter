using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class WhileNode : DecisionNode
    {
        public WhileNode()
        {
            base.Name = "While";
            base.setText("While");
        }

        public override void shiftMainTrack(int moreShift = 0)
        {
            if (base.OutConnector.EndNode != null)
                base.OutConnector.EndNode.shiftDown(moreShift);
        }

        protected override void makeConnections()
        {
            base.TrueNode = new HolderNode(this);
            base.TrueConnector = new ConnectorNode(this);
            base.TrueConnector.Connector.Opacity = 50;
            base.TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            base.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            base.BackNode = new HolderNode(this);
            base.BackNode.OutConnector.EndNode = this;
            base.BackNode.OutConnector.Connector.Opacity = 50;
            base.TrueNode.OutConnector.EndNode = base.BackNode;
            base.TrueConnector.Selectable = false;
            base.BackNode.OutConnector.Selectable = false;
            base.BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");
        }

        protected override void moveConnections()
        {
            PointF point = new PointF(base.Shape.Width + base.Shape.Location.X + horizontalSpace, base.Shape.Center.Y - base.TrueNode.Shape.Size.Height / 2);
            PointF oldPlace = base.TrueNode.NodeLocation;

            base.TrueNode.NodeLocation = point;

            if (base.TrueConnector.EndNode == null)
            {
                base.TrueConnector.EndNode = base.TrueNode;
                base.TrueNode.OutConnector.EndNode = base.BackNode;
            }

            if (base.TrueNode.OutConnector.EndNode is HolderNode)
            {
                if (base.moveDirection == MOVE_UP)
                    base.TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point.Y);
                else
                    base.BackNode.NodeLocation = new PointF(point.X, point.Y + 60);
            }

            else
            {
                base.BackNode.NodeLocation = new PointF(point.X, base.BackNode.NodeLocation.Y);

                if (base.moveDirection == MOVE_DOWN)
                    base.TrueNode.OutConnector.EndNode.shiftDown(moreShift);

                else if (base.moveDirection == MOVE_UP)
                    base.TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point.Y);
            }
        }

        public override void onShapeClicked()
        {
            //To be implemented 
        }
    }
}
