using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ResizeAndUpload.Controller
{
    public class Settings
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string EncryptedPassword { get; set; }

        public static Settings Load()
        {
            if (!File.Exists("settings.xml"))
                return new Settings() { EncryptedPassword = "", Url = "", User = "" };

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (XmlTextReader reader = new XmlTextReader("settings.xml"))
            {
                return serializer.Deserialize(reader) as Settings;
            }
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (XmlTextWriter writer = new XmlTextWriter("settings.xml", Encoding.UTF8))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
