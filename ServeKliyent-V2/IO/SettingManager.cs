using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ServeKliyent_V2.Utils;
using System.Xml.Serialization;
using System.Xml;

namespace ServeKliyent_V2.IO
{
    public class SettingManager
    {
        public Settings settings { get; set; }

        [Obsolete]
        private static void SaveDataOld(Settings data, string filePath) //TODO: Make this XML or JSON
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        [Obsolete]
        private static Settings LoadDataOld(string filePath) //TODO: Make this XML or JSON
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Settings obj = (Settings)formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }

        private static void SaveData(Settings data, string filePath) //TODO: Make this XML or JSON
        {
            var serializer = new XmlSerializer(data.GetType());
            using (var writer = XmlWriter.Create(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        private static Settings LoadData(string filePath) //TODO: Make this XML or JSON
        {
            Settings obj = new Settings();
            var serializer = new XmlSerializer(typeof(Settings));
            using (var reader = XmlReader.Create(filePath))
            {
                obj = (Settings)serializer.Deserialize(reader);
            }
            return obj;
        }

        public bool SaveSettings(string savePath)
        {
            try
            {
                SaveData(settings, savePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool loadSettings(string savePath)
        {
            try
            {
                settings = LoadData(savePath);
                return true;
            }
            catch (Exception ex)
            {
                settings = new Settings(); //Nothing loaded so init the object with default values

                settings.outputMode = OutputMode.Essential;

                return false;
            }
        }

        public SettingManager()
        {
            loadSettings(Environment.CurrentDirectory + "/server.properties");
        }

        public SettingManager(string filePath)
        {
            loadSettings(filePath);
        }
    }
}
