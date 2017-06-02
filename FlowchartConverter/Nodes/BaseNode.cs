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
        private Form dialog;
        private bool toBeRemoved = false;

        private Shape shape;
        private BaseNode parentNode;
        private FlowchartStencil stencil;

        private String statement;
        private String shapeTag;
        private String connectorTag;

        protected PointF nodeLocation;

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

        public string Name { get; internal set; }

        public BaseNode()
        {
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
            Shape.Label = new Crainiate.Diagramming.Label(label);
            SizeF size;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(label, Shape.Label.Font);
            }

            float oldWidth = Shape.Size.Width;
            Shape.Size = new SizeF(size.Width + 50, Shape.Size.Height);
            Shape.Label.Color = Color.White;
        }

        public virtual void addRemoveFlag(bool v)
        {
            toBeRemoved = v;
        }

        public virtual void onShapeClicked()
        {
            //To be implemented
        }
    }
}