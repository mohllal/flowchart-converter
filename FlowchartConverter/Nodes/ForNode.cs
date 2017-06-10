using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class ForNode : WhileNode
    {
        public ForNode()
        {
            base.Name = "For";
            base.setText("For");
            base.TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("Next");
            base.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }
    }
}
