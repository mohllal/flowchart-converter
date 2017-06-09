using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    abstract class DecisionNode : BaseNode
    {
        private HolderNode trueNode;
        private HolderNode backNode;
        protected ConnectorNode trueConnector;

        private readonly string holderTag;
        private readonly string backConnectorTag;
        private readonly string trueConnectorTag;

        protected const int horizontalSpace = 100;
        protected const int MOVE_UP = 1;
        protected const int MOVE_DOWN = 2;
        protected const int MOVE_RIGHT = 3;
        protected int moveDirection = MOVE_DOWN;

        public bool shifted = false;

        public HolderNode TrueNode
        {
            get
            {
                return trueNode;
            }

            set
            {
                trueNode = value;
            }
        }

        public HolderNode BackNode
        {
            get
            {
                return backNode;
            }

            set
            {
                backNode = value;
            }
        }

        public ConnectorNode TrueConnector
        {
            get
            {
                return trueConnector;
            }

            set
            {
                trueConnector = value;
            }
        }

        public override PointF NodeLocation
        {
            get
            {
                return nodeLocation;
            }

            set
            {
                if (value.X != this.NodeLocation.X || value.Y != this.NodeLocation.Y)
                {
                    if (value.Y > this.NodeLocation.Y)
                        this.moveDirection = MOVE_DOWN;

                    else if (value.X > this.NodeLocation.X)
                        this.moveDirection = MOVE_RIGHT;

                    else
                        this.moveDirection = MOVE_UP;

                    base.NodeLocation = value;
                    this.moveConnections();
                }
            }
        }

        public DecisionNode()
        {
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Preparation];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e06000");
            base.Shape.GradientColor = Color.Black;

            this.holderTag = base.ShapeTag + " holder";
            this.backConnectorTag = base.ShapeTag + " backConnector";
            this.trueConnectorTag = base.ShapeTag + " trueConnector";

            this.makeConnections();
        }

        public override void addRemoveFlag(bool v)
        {
            this.BackNode.ToBeRemoved = true;
            this.TrueNode.ToBeRemoved = true;

            BaseNode nextNode = TrueNode;

            while (nextNode.OutConnector.EndNode != this.BackNode)
            {
                nextNode.OutConnector.EndNode.addRemoveFlag(true);
                nextNode = nextNode.OutConnector.EndNode;
            }

            nextNode = null;
            this.ToBeRemoved = true;
        }

        public override void addToModel()
        {
            base.addToModel();
            this.TrueNode.addToModel();
            this.BackNode.addToModel();
        }

        protected void attachToTrue(BaseNode newNode, bool addToEnd)
        {
            if (addToEnd)
            {
                BaseNode lastNode = this.TrueNode;
                while (!(lastNode.OutConnector.EndNode is HolderNode))
                    lastNode = lastNode.OutConnector.EndNode;
                lastNode.attachNode(newNode);
            }

            else
                this.TrueNode.attachNode(newNode);
        }

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            clickedConnector.StartNode.attachNode(newNode);

            if (base.OutConnector.EndNode.NodeLocation.Y < this.BackNode.NodeLocation.Y + shiftY)
                this.shiftMainTrack();
        }

        public override void setText(String label)
        {
            float oldWidth = Shape.Width;
            base.setText(label);

            if (!Controller.LoadingProject)
            {
                this.TrueNode.shiftRight((int)(base.Shape.Width - oldWidth));
                this.BackNode.shiftRight((int)(base.Shape.Width - oldWidth));
            }
        }

        public abstract void shiftMainTrack(int moreShift = 0);

        protected abstract void makeConnections();

        protected abstract void moveConnections();
    }
}
