using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter.Nodes
{
    class BaseNode : Crainiate.Diagramming.OnShapeClickListener
    {
        private static MainForm form;
        private static FlowchartConverter.Main.Controller controller;

        private Form dialog;
        private bool toBeRemoved = false;

        private Shape shape;
        private BaseNode parentNode;
        private ConnectorNode outConnector;
        private FlowchartStencil stencil;

        private String statement;
        private String shapeTag;
        private String connectorTag;

        protected PointF nodeLocation;
        protected int shiftY = 85;
        protected float moreShift = 0;
        private float rightShifCaused = 0;

        private static int counter;

        public static MainForm Form
        {
            get
            {
                return form;
            }

            set
            {
                form = value;
            }
        }

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

        public Form Dialog
        {
            get
            {
                return dialog;
            }

            set
            {
                dialog = value;
            }
        }

        public bool ToBeRemoved
        {
            get
            {
                return toBeRemoved;
            }

            set
            {
                toBeRemoved = value;
            }
        }

        public Shape Shape
        {
            get
            {
                return shape;
            }
        }

        public BaseNode ParentNode
        {
            get
            {
                return parentNode;
            }

            set
            {
                parentNode = value;
            }
        }

        public ConnectorNode OutConnector
        {
            get
            {
                return outConnector;
            }

            set
            {
                outConnector = value;
            }
        }

        public FlowchartStencil Stencil
        {
            get
            {
                return stencil;
            }

            set
            {
                stencil = value;
            }
        }

        public string Statement
        {
            get
            {
                return statement;
            }

            set
            {
                statement = value;
                showStatment();
            }
        }

        public string ShapeTag
        {
            get
            {
                return shapeTag;
            }

            set
            {
                shapeTag = value;
            }
        }

        public string ConnectorTag
        {
            get
            {
                return connectorTag;
            }

            set
            {
                connectorTag = value;
            }
        }

        public virtual PointF NodeLocation
        {
            get
            {
                return nodeLocation;
            }

            set
            {
                nodeLocation = value;
                connectedShape().Location = value;
            }
        }

        public float RightShifCaused
        {
            get
            {
                return rightShifCaused;
            }

            set
            {
                rightShifCaused = value;
            }
        }

        public string Name { get; internal set; }

        public BaseNode()
        {
            if (Controller == null)
                throw new Exception("Controller Must Be set First");
            this.shape = new Shape();
            this.Shape.Label = new Crainiate.Diagramming.Label();
            this.Shape.Label.Color = Color.White;
            this.Shape.KeepAspect = false;
            this.Shape.ShapeListener = this;
            this.Shape.AllowMove = Shape.AllowScale = Shape.AllowRotate = Shape.AllowSnap = false;
            this.Shape.Size = new SizeF(80, 50);
            this.Shape.KeepAspect = false;
            this.Shape.Label.Color = Color.White;
            this.Stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));
            this.OutConnector = new ConnectorNode(this);
            counter = counter + 1;
            this.ShapeTag = "Shape_" + counter.ToString();
            this.ConnectorTag = "Connector_" + counter.ToString();
            
        }

        public BaseNode(PointF location) : this()
        {
            NodeLocation = location;
        }

        protected virtual void showStatment()
        {
            if (!String.IsNullOrEmpty(Statement))
                setText(Statement);
        }

        public virtual Shape connectedShape()
        {
            return Shape;
        }

        public virtual void setText(String label)
        {
            this.Shape.Label = new Crainiate.Diagramming.Label(label);
            SizeF size;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(label, Shape.Label.Font);
            }

            float oldWidth = this.Shape.Size.Width;
            this.Shape.Size = new SizeF(size.Width + 50, Shape.Size.Height);
            this.Shape.Label.Color = Color.White;
            if (!Controller.LoadingProject)
                Controller.shiftNodesRight(this, true, (int)(this.Shape.Size.Width - oldWidth));
        }

        public virtual void addRemoveFlag(bool v)
        {
            this.toBeRemoved = v;
        }

        public virtual void addToModel()
        {
            Controller.addToModel(this);
        }

        public virtual void removeFromModel()
        {
            addRemoveFlag(true);
            Controller.removeNode(this);
        }

        public void attachNode(BaseNode newNode)
        {
            if (this is TerminalNode && newNode is TerminalNode || this is HolderNode && newNode is HolderNode)
            {
                if (newNode.connectedShape() == null) ;
                //do nothing
                if (this.connectedShape() == null) ;
                    //donothing
                this.OutConnector.EndNode = newNode;
                newNode.NodeLocation = this.NodeLocation;
                newNode.shiftDown(0);
                return;

            }

            BaseNode oldOutNode = this.OutConnector.EndNode;
            this.OutConnector.EndNode = newNode;

            float x = this.NodeLocation.X;
            float y = oldOutNode.NodeLocation.Y;

            if (this.NodeLocation.X != oldOutNode.NodeLocation.X)
            {
                if (this.ParentNode is IfElseNode && this == (this.parentNode as IfElseNode).MiddleNode)
                    x = this.parentNode.NodeLocation.X;
                else if (this is HolderNode)
                    x = oldOutNode.NodeLocation.X;
                else if (oldOutNode is HolderNode)
                    x = this.NodeLocation.X;
            }

            newNode.OutConnector.EndNode = oldOutNode;
            oldOutNode.shiftDown(0);
            newNode.NodeLocation = new PointF(x, y);
            controller.balanceNodes(newNode);
        }

        public virtual void attachNode(BaseNode newNode, ConnectorNode connectorNode)
        {
            this.attachNode(newNode);
        }

        public virtual void shiftDown(float moreShift = 0)
        {
            if (this.connectedShape() != null)
                this.NodeLocation = new PointF(this.connectedShape().Location.X, this.connectedShape().Location.Y + shiftY + moreShift);

            if (!(this is HolderNode) && this.OutConnector.EndNode != null)
                this.OutConnector.EndNode.shiftDown(moreShift);
        }

        public void shiftUp(float offsetY)
        {
            this.NodeLocation = new PointF(this.NodeLocation.X, this.NodeLocation.Y - offsetY);

            if (this.OutConnector.EndNode == null || this is DecisionNode)
                return;

            if (this is HolderNode)
            {
                if (this.ParentNode is IfElseNode)
                {
                    IfElseNode pNode = this.ParentNode as IfElseNode;
                    PointF preLocation = pNode.MiddleNode.NodeLocation;
                    pNode.balanceHolderNodes();
                    if (pNode.MiddleNode.NodeLocation.Y == preLocation.Y)
                        return;
                }

                else if (this.ParentNode is IfNode)
                {
                    IfNode pNode = (this.ParentNode as IfNode);
                    pNode.MiddleNode.NodeLocation = new PointF(pNode.MiddleNode.connectedShape().Location.X, pNode.BackNode.NodeLocation.Y);
                }
                this.ParentNode.OutConnector.EndNode.shiftUp(offsetY);

            }
            else
                this.OutConnector.EndNode.shiftUp(offsetY);


        }

        public void shiftRight(int distance)
        {
            this.NodeLocation = new PointF(this.NodeLocation.X + distance, this.NodeLocation.Y);
        }

        public virtual void onShapeClicked()
        {
            if (this.Shape.Selected && Controller.DeleteChoosed)
            {

                this.removeFromModel();
                Controller.DeleteChoosed = false;
                this.Shape.Selected = false;
            }
        }
    }
}