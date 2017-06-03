using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class IfElseNode : IfNode
    {
        private HolderNode falseNode;
        private HolderNode backfalseNode;
        private ConnectorNode falseConnector;

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

        public IfElseNode()
        {
            base.Name = "IfElse";
            base.Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            base.Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            base.Shape.GradientColor = Color.Black;
            base.setText("IF");

        }

        public override void onShapeClicked()
        {
            //To be implemented
        }
    }
}
