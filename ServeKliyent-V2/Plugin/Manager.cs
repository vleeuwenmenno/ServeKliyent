using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ServeKliyent_V2.Utils;

namespace ServeKliyent_V2.Plugin
{
    public class Manager
    {
        public List<Plugin> loadedPlugins { get; set; }
        public bool loaded { get; set; }

        public void LoadPlugins()
        {
            if (!loaded)
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

                    plg.types = plg.assembly.GetExportedTypes();

                    int mainType = 0;

                    foreach (Type t in plg.types)
                    {
                        if (plg.types[mainType].Name == "Plugin")
                            break;

                        mainType++;
                    }

                    plg.methods = plg.types[mainType].GetMethods();

                    //Let the plugin init herself.
                    plg = (Plugin)plg.Execute("Start");
                    plg.pluginInstance = plg;
                    loadedPlugins.Add(plg);

                    loadCount++;
                }

                loaded = true;
                Program.console.WriteLine("Successfully loaded " + loadCount + " plugin(s).", LogLevel.Info);
            }
            else
            {
                Program.console.WriteLine("Cannot load plugins when there are already plugins loaded!", LogLevel.Warning);
            }
        }

        public void UnloadPlugins()
        {
            if (loaded)
            {
                Program.console.WriteLine("Stopping plugins...", LogLevel.Info);

                for (int i = 0; i < loadedPlugins.Count; i++)
                {
                    loadedPlugins[i].Execute("Stop");
                    loadedPlugins.RemoveAt(i);
                }

                loaded = false;
                Program.console.WriteLine("Successfully stopped all plugins.", LogLevel.Info);
            }
            else
            {
                Program.console.WriteLine("Cannot unload plugins when there are none loaded!", LogLevel.Warning);
            }
        }
    }
}
