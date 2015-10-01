using ResizeAndUpload.UI;
using ResizeAndUpload.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResizeAndUpload.Controller
{
    public class ResizeAndUpload
    {
        private Settings settings;
        private string galleryName;
        private string sourceDir;
        private int maxImgSize;
        FormStart mainForm;
        private string[] files;

        public ResizeAndUpload(FormStart mainForm, Settings settings, string sourceDir, string galleryName, string maxImgSize)
        {
            this.settings = settings;
            this.sourceDir = sourceDir;
            this.galleryName = galleryName;
            int.TryParse(maxImgSize, out this.maxImgSize);
            this.mainForm = mainForm;
            files = Directory.GetFiles(sourceDir, "*.jpg");
        }

        public void Run()
        {
            mainForm.Enabled = false;

            Ftp.CreateDir(settings, galleryName);
            mainForm.StatusText = "Verzeichnis erstellt ...";

            mainForm.ProgressBar.Minimum = 0;
            mainForm.ProgressBar.Maximum = files.Length;
            mainForm.ProgressBar.Step = 1;
            mainForm.ProgressBar.Value = 0;

            BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainForm.Enabled = true;

            mainForm.StatusText = "Fertig!";

            mainForm.ProgressBar.Minimum = 0;
            mainForm.ProgressBar.Maximum = files.Length;
            mainForm.ProgressBar.Step = 1;
            mainForm.ProgressBar.Value = 0;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var status = e.UserState.ToString().Split(';');
            mainForm.StatusText = status[1];

            int val = 0;
            int.TryParse(status[0], out val);

            mainForm.ProgressBar.Value = val;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            byte[] newImg;
            int count = 1;
            foreach (var file in files)
            {
                worker.ReportProgress(1, string.Format("{0};Bearbeite Bild #{0} ...", count));
                newImg = Img.ResizeImage(file, maxImgSize);

                worker.ReportProgress(1, string.Format("{0};Lade Bild #{0} hoch ...", count));
                Ftp.UploadFile(settings, galleryName, Path.GetFileName(file), newImg);

                count++;
            }
        }
    }
}
