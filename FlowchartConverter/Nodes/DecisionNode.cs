using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class DecisionNode : BaseNode
    {
        private HolderNode trueNode;
        private HolderNode backNode;
        protected ConnectorNode trueConnector;

        private readonly string holderTag;
        private readonly string backConnectorTag;
        private readonly string trueConnectorTag;


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

        public DecisionNode()
        {
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Preparation];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e06000");
            base.Shape.GradientColor = Color.Black;

            this.holderTag = base.ShapeTag + " holder";
            this.backConnectorTag = base.ShapeTag + " backConnector";
            this.trueConnectorTag = base.ShapeTag + " trueConnector";
        }
    }
}
