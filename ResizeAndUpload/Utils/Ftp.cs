using ResizeAndUpload.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResizeAndUpload.Utils
{
    public class Ftp
    {
        public static void UploadFile(Settings settings, string dir, string fileName, byte[] fileContent)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Credentials = new NetworkCredential(settings.User, Crypto.DecryptString(settings.EncryptedPassword));
                wc.UploadData(settings.Url + dir + "/" + fileName, fileContent);
            }
        }

        public static void CreateDir(Settings settings, string dir)
        {
            WebRequest req = WebRequest.Create(settings.Url + dir);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            req.Credentials = new NetworkCredential(settings.User, Crypto.DecryptString(settings.EncryptedPassword));
            using (WebResponse resp = req.GetResponse()) { }
        }
    }
}
