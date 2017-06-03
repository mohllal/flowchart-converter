using Crainiate.Diagramming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class ConnectorNode : Crainiate.Diagramming.OnShapeClickListener
    {
        private BaseNode startNode;
        private BaseNode endNode;
        private Connector connector = new Connector();
        private bool selectable = true;

        public Connector Connector
        {
            get
            {
                return connector;
            }

            set
            {
                connector = value;
            }
        }

        public BaseNode StartNode
        {
            get
            {
                return startNode;
            }

            set
            {
                connector.Start.Shape = value.connectedShape();
                startNode = value;
            }
        }

        public BaseNode EndNode
        {
            get
            {
                return endNode;
            }

            set
            {
                endNode = value;
                connector.End.Shape = value.connectedShape();

            }
        }

        public bool Selectable
        {
            get
            {
                return selectable;
            }

            set
            {
                selectable = value;
            }
        }

        public ConnectorNode(BaseNode startNode)
        {
            
            this.startNode = startNode;
            this.connector.Start.Shape = startNode.Shape;
            this.connector.OnShapeSelectedListener = this;
            this.connector.End.Marker = new Arrow();
            this.connector.AllowMove = true;
            this.connector.Avoid = connector.Jump = false;
            this.connector.SetOrder(1);
            this.connector.DrawSelected = true;
        }

        public void setEndNode(BaseNode endNode)
        {
            this.endNode = endNode;
        }

        public void onShapeClicked()
        {
            //To be implemented
        }
    }
}
