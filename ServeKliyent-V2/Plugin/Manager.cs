using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServeKliyent_V2.Utils;

namespace ServeKliyent_V2.Plugin
{
    public class Manager
    {
        public List<Plugin> loadedPlugins { get; set; }

        public void LoadPlugins()
        {
            Program.console.WriteLine("Loading plugins...", LogLevel.Info);

            if (!Directory.Exists(Environment.CurrentDirectory + "/Plugins"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "/Plugins");

            loadedPlugins = new List<Plugin>();
            int loadCount = 0;

            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/Plugins");
            foreach (FileInfo file in dir.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                Plugin plg = new Plugin();
                plg.assembly = Assembly.LoadFile(file.FullName);

                loadedPlugins.Add(plg);
                loadCount++;
            }

            Program.console.WriteLine("Successfully loaded " + loadCount + " plugin(s).", LogLevel.Info);
        }

        public void UnloadPlugins()
        {

        }
    }
}
