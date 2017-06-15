using Crainiate.Diagramming;
using FlowchartConverter.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter.Nodes
{
    class ConnectorNode : Crainiate.Diagramming.OnShapeClickListener
    {
        private static FlowchartConverter.Main.Controller controller;

        private BaseNode startNode;
        private BaseNode endNode;
        private Connector connector = new Connector();
        private bool selectable = true;

        public static FlowchartConverter.Main.Controller Controller
        {
            get
            {
                return controller;
            }

            set
            {
                controller = value;
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
            if (Controller == null)
                throw new Exception("Controller must be set to use Connectors");

            this.startNode = startNode;
            this.connector.Start.Shape = this.startNode.Shape;
            this.connector.OnShapeSelectedListener = this;
            this.connector.End.Marker = new Arrow();
            this.connector.AllowMove = true;
            this.connector.Avoid = this.connector.Jump = false;
            this.connector.SetOrder(1);
            this.connector.DrawSelected = true;
        }

        public void setEndNode(BaseNode endNode)
        {
            this.endNode = endNode;
        }

        public void addNewNode(BaseNode toAttachNode)
        {
            if (toAttachNode != null)
            {
                if (this.checkIfHolderExist() != null)
                {
                    toAttachNode.ParentNode = (checkIfHolderExist()).ParentNode;
                    toAttachNode.ParentNode.attachNode(toAttachNode, this);
                }

                else
                {
                    toAttachNode.ParentNode = startNode.ParentNode;
                    startNode.attachNode(toAttachNode);
                }

                toAttachNode.addToModel();
            }
        }

        private HolderNode checkIfHolderExist()
        {
            if (this.startNode is HolderNode)
                return (HolderNode)this.startNode;

            BaseNode testNode = this.endNode;

            while (testNode.OutConnector.EndNode != null)
            {
                if (testNode is HolderNode)
                    return (HolderNode)testNode;

                testNode = testNode.OutConnector.EndNode;
            }
            return null;
        }

        public static BaseNode getPickedNode()
        {
            BaseNode toAttachNode = null;
            PickerDialog pd = new PickerDialog();
            DialogResult res = pd.ShowDialog();
            if (res != DialogResult.OK)
                return null;
            toAttachNode = pd.SelectedShape;

            return toAttachNode;
        }

        public void onShapeClicked()
        {
            if (!this.connector.Selected || !this.selectable || Controller.AllowMove == true)
                return;
            BaseNode node = getPickedNode();
            if (node == null)
                return;
            this.addNewNode(node);
            this.connector.Selected = false;
        }
    }
}
