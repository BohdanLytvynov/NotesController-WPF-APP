using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;


namespace ConfigController
{
    public static class Congiguration
    {
        //Path to xml file with setting(stores localhost info)
        private static string path = Environment.CurrentDirectory + @"\Settings\Config.xml";
        /// <summary>
        /// Writes localhost info to file
        /// </summary>
        /// <param name="settings"></param>
        public static void WriteSettingsToFileXML(Dictionary<string, string> settings)
        {
            CheckIfFileExists();

            Write(settings);
        }
        /// <summary>
        /// Creates xml document and writes it to file
        /// </summary>
        /// <param name="settings"></param>
        private static void Write(Dictionary<string, string> settings)
        {
            XDocument doc = new XDocument();

            XElement set = new XElement("settings");

            foreach (var pair in settings)
            {
                XElement element = new XElement("add");

                XAttribute attribute = new XAttribute(pair.Key, pair.Value);

                element.Add(attribute);

                set.Add(element);
            }

            doc.Add(set);

            doc.Save(path);
        }
        /// <summary>
        /// Reads xml file with localhost settings
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Read()
        {
            CheckIfFileExists();

            FileInfo file = new FileInfo(path);

            if (file.Length ==0 )
            {
                return new Dictionary<string, string>();
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            string xml = File.ReadAllText(path);

            XDocument doc = XDocument.Parse(xml);

            XElement settings = doc.Root;

            IEnumerable<XNode> addNodes = settings.DescendantNodes();

            foreach (var item in addNodes)
            {
                IEnumerable<XAttribute> atrib = (item as XElement).Attributes();

                foreach (var a in atrib)
                {
                    result.Add(a.Name.ToString(), a.Value);
                }
            }
            
            return result;
            
        }
        /// <summary>
        /// Checks if ifle exist
        /// </summary>
        private static void CheckIfFileExists()
        {
            FileInfo file = new FileInfo(path);

            if (!file.Exists)
            {
                FileStream f = File.Create(path);
                f.Close();
                f.Dispose();
            }
        }
    }
}
