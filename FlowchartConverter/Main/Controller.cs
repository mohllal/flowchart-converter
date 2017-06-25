using Crainiate.Diagramming;
using Crainiate.Diagramming.Forms;
using FlowchartConverter.Nodes;
using FlowchartConverter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter.Main
{
    class Controller
    {
        private Model model;
        private Diagram diagram;

        private List<BaseNode> nodes = new List<BaseNode>();
        private BaseNode nodeInitiateRemoving;

        private TerminalNode terminalE;
        private TerminalNode terminalS;

        private bool deleteChoosed;
        private bool allowMove;

        public Model Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public List<BaseNode> Nodes
        {
            get
            {
                return nodes;
            }

            set
            {
                nodes = value;
            }
        }

        public bool DeleteChoosed
        {
            get
            {
                return deleteChoosed;
            }

            set
            {
                deleteChoosed = value;
            }
        }

        public bool AllowMove
        {
            get
            {
                return allowMove;
            }

            set
            {
                allowMove = value;
            }
        }

        public bool LoadingProject { get; internal set; }

        public enum Language { CPP, CSHARP };

        public Controller(Diagram diagram1)
        {
            this.diagram = diagram1;
            this.Model = diagram1.Model;
            this.initializeProject();
            new OutputNode();
        }

        public void initializeProject()
        {
            this.Model.Clear();
            this.Nodes.Clear();
            BaseNode.Controller = this;
            ConnectorNode.Controller = this;
            this.terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            this.terminalE = new TerminalNode(TerminalNode.TerminalType.End);
            this.terminalS.attachNode(terminalE);
            this.terminalE.ParentNode = terminalS;
            this.terminalS.addToModel();
            this.terminalE.addToModel();
        }

        public void newProject()
        {
            initializeProject();
            diagram.Controller.Refresh();
        }

        public void saveProject(string path)
        {
            Saver ps = new Saver(this, terminalS, terminalE, path);
        }

        public void loadProject(string path)
        {
            this.LoadingProject = true;
            this.initializeProject();
            Loader projectLoader = new Loader(terminalS, terminalE, path);
            this.diagram.Controller.Refresh();
            this.LoadingProject = false;
        }

        public string getCode(Language lang)
        {
            switch (lang)
            {
                case Language.CPP:
                    return CodeGenerator.getCppCode(terminalS, terminalE);
                case Language.CSHARP:
                    return CodeGenerator.getCsCode(terminalS, terminalE);
            }
            return null;
        }

        public void cancelClickedButtons()
        {
            this.DeleteChoosed = false;
            this.AllowMove = false;
        }

        public void shiftNodesRight(BaseNode shiftNode, bool exculdeNode, int distance = 150)
        {
            shiftNode.RightShifCaused += distance;
            foreach (BaseNode node in this.Nodes)
            {
                if (node is HolderNode)
                    continue;

                if (exculdeNode)
                    if (node.NodeLocation.X > shiftNode.NodeLocation.X)
                        node.shiftRight(distance);

                    else if (node.NodeLocation.X >= shiftNode.NodeLocation.X)
                        node.shiftRight(distance);
            }
        }

        public void redrawNodes()
        {
            this.Model.Clear();

            foreach (BaseNode node in Nodes)
            {
                if (node is HolderNode) continue;
                node.addToModel();
            }
            this.diagram.Controller.Refresh();
        }

        public void balanceNodes(BaseNode newNode)
        {
            BaseNode trackNode = null;

            do
            {
                if (trackNode == null)
                    trackNode = newNode.ParentNode;

                else
                    trackNode = trackNode.ParentNode;

                if (newNode is IfElseNode)
                {
                    if (trackNode.NodeLocation.X < newNode.NodeLocation.X
                     && (newNode as IfElseNode).FalseNode.NodeLocation.X <= trackNode.NodeLocation.X + trackNode.Shape.Width)
                    {
                        this.addNode(newNode);
                        this.shiftNodesRight(newNode, false);
                    }
                }
                if (newNode is DecisionNode)
                {
                    if (trackNode.NodeLocation.X > newNode.NodeLocation.X && (newNode as DecisionNode).TrueNode.NodeLocation.X > trackNode.NodeLocation.X)
                        this.shiftNodesRight(newNode, true);
                }

                else if (trackNode.NodeLocation.X > newNode.NodeLocation.X && newNode.Shape.Width + newNode.NodeLocation.X > trackNode.NodeLocation.X)
                    this.shiftNodesRight(newNode, true, 100);
            }
            while (!(trackNode is TerminalNode));
        }

        public void addToModel(BaseNode toAddNode)
        {
            if (this.Model == null)
                throw new Exception("Model should be set before calling addToModel");

            if (toAddNode.OutConnector.EndNode != null)
                this.Model.Lines.Add(toAddNode.ConnectorTag, toAddNode.OutConnector.Connector);

            if (toAddNode.Shape != null)
                this.Model.Shapes.Add(toAddNode.ShapeTag, toAddNode.Shape);

            this.addNode(toAddNode);

            if (toAddNode is IfElseNode && toAddNode.NodeLocation.X < 100)
                this.shiftNodesRight(toAddNode, true);

            if (toAddNode is DecisionNode)
                this.Model.Lines.Add((toAddNode as DecisionNode).TrueConnector.Connector);

            if (toAddNode is IfElseNode)
                this.Model.Lines.Add((toAddNode as IfElseNode).FalseConnector.Connector);
        }

        public void removeNode(BaseNode toRemoveNode)
        {
            if (this.Model == null)
                throw new Exception("Model should be set before calling addToModel");

            foreach (BaseNode node in Nodes)
            {
                BaseNode nextNode = node.OutConnector.EndNode;
                if (nextNode == toRemoveNode && node.OutConnector.EndNode != node.ParentNode)
                {
                    node.OutConnector.EndNode = nextNode.OutConnector.EndNode;
                    node.OutConnector.EndNode.shiftUp(node.OutConnector.EndNode.NodeLocation.Y - toRemoveNode.NodeLocation.Y);
                    break;
                }
            }

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (this.Nodes[i].ToBeRemoved)
                {
                    this.Nodes.Remove(this.Nodes[i]);
                    i--;
                }
            }
            this.redrawNodes();
        }

        public void addNode(BaseNode node)
        {
            if (!this.nodes.Contains(node))
            {
                this.nodes.Add(node);
                if (node is IfElseNode && node.NodeLocation.X < 100)
                    this.shiftNodesRight(node, false, 150);
            }
        }

        public void showErrorMsg(string str)
        {
            MessageBox.Show("Error in Controller Class");
        }
    }
}
