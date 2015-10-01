using ResizeAndUpload.Controller;
using System;
using System.Windows.Forms;

namespace ResizeAndUpload.UI
{
    public partial class FormStart : Form
    {
        private Settings settings;
        public string StatusText
        {
            get
            {
                return lblStatus.Text;
            }
            set
            {
                lblStatus.Text = value;
            }
        }
        public ProgressBar ProgressBar { get { return progressBar; } }

        public FormStart()
        {
            InitializeComponent();
            settings = Settings.Load();
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            using (FormSettings frmSettings = new FormSettings(settings))
            {
                frmSettings.ShowDialog();
            }
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    txtPath.Text = fbd.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            new Controller.ResizeAndUpload(this, settings, txtPath.Text, txtGalleryName.Text, txtImgSize.Text).Run();
        }
    }
}
