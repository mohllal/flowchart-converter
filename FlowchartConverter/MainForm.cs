using Crainiate.Diagramming;
using Crainiate.Diagramming.Forms;
using FlowchartConverter.Main;
using FlowchartConverter.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter
{
    public partial class MainForm : Form
    {
        private Main.Controller controller;
        private TerminalNode terminalS;
        private TerminalNode terminalE;

        public MainForm()
        {
            InitializeComponent();
            controller = new Main.Controller(diagram1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void diagram1_MouseClick_1(object sender, MouseEventArgs e)
        {
        }

        private void xmlBtn_Click(object sender, EventArgs e)
        {
        }

        private void onLoad_click(object sender, EventArgs e)
        {
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            controller.newProject();
        }

        private void DialogsBtn_Click(object sender, EventArgs e)
        {
            controller.AllowMove = true;
        }

        private void open_button_Click(object sender, EventArgs e)
        {
        }

        private void save_button_Click(object sender, EventArgs e)
        {
        }

        private void move_button_Click(object sender, EventArgs e)
        {
            controller.AllowMove = true;
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            controller.newProject();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            controller.DeleteChoosed = true;
        }

        private void sourceCodeButton_Click(object sender, EventArgs e)
        {
        }

        private void export_button_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                Crainiate.Diagramming.Forms.FormsDocument _fd = new FormsDocument(diagram1.Model);
                _fd.Export(fbd.SelectedPath + "output.jpg", ExportFormat.Jpeg);
            }
        }
    }
}
