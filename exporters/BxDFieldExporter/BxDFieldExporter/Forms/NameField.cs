using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BxDFieldExporter.Forms
{
    public partial class NameFieldForm : Form
    {
        public enum NameMode { Initial, SaveAs }
        private NameMode nameMode;

        public NameFieldForm(NameMode mode)
        {
            InitializeComponent();

            nameMode = mode;

            txtDirectory.Text = SynthesisGUI.PluginSettings.GeneralSaveLocation;
        }

        public static DialogResult NameField(out string FieldName, NameMode mode = NameMode.SaveAs)
        {
            try
            {
                NameFieldForm form = new NameFieldForm(mode);
                form.ShowDialog();
                FieldName = form.txtName.Text;
                return form.DialogResult;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            string path = txtDirectory.Text;

            if(txtName.Text != null)
            {
                string name = txtName.Text;

                if (File.Exists(SynthesisGUI.PluginSettings.GeneralSaveLocation + "\\" + txtName.Text + @"\skeleton.bxdf") && MessageBox.Show("Overwrite Existing Field?", "Save Field", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a name for your field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult warningResult = MessageBox.Show("Are you sure you want to cancel? (All export progress would be lost)",
                "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if ((nameMode == NameMode.Initial && warningResult == DialogResult.Yes) || nameMode == NameMode.SaveAs)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (ExportLocationDialog.ShowDialog() == DialogResult.OK)
            {
                PathTextBox.Text = ExportLocationDialog.SelectedPath;
                SynthesisGUI.PluginSettings.GeneralSaveLocation = ExportLocationDialog.SelectedPath;
            }
        }

        private void ExportLocationDialog_HelpRequest(object sender, EventArgs e)
        {
            if (ExportLocationDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = ExportLocationDialog.SelectedPath;
            }
        }
    }
}
