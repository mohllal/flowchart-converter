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

        public MainForm()
        {
            InitializeComponent();
            controller = new Main.Controller(diagram1);
        }

        private void diagram1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                controller.cancelClickedButtons();
            }
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
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Xml Source (*.xml)|*.xml";
            string path = null;
            if (DialogResult.OK == opf.ShowDialog())
            {
                path = opf.FileName;
            }
            else
            {
                return;
            }

            controller.loadProject(path);
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml Source (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                controller.saveProject(saveFileDialog.FileName);
            else
                return;
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
            string cppStr = controller.getCode(Main.Controller.Language.CPP);
            string cSharpStr = controller.getCode(Main.Controller.Language.CSHARP);
            CodeForm code = new CodeForm();
            code.setMeta(cppStr, cSharpStr);
            code.set(cppStr);
            code.Show();
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
