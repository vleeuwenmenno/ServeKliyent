using ServeKliyent_V2.Utils;
using ServeKliyent_V2.CommandManagers;
using System;

namespace PlusCommands
{
    public class Plugin : ServeKliyent_V2.Plugin.Plugin
    {       
        public ServeKliyent_V2.Plugin.Plugin Start()
        {
            //Initalize the plugin at the pluginManager
            Init(Guid.NewGuid());
            pluginName = "PlusCommands";

            //Register commands
            Command cmd = new Command(this, pluginName);

            //Define the command info
            cmd.command = "plus";
            cmd.method = "PlusExecute";
            cmd.usage = "Display 1 + 1 :P";

            //Finally register the command in the commandManager
            commandMan.RegisterCommand(cmd);

            //Tell the user that we are loaded
            log.WriteLine("Plugin has been loaded!", LogLevel.Info, true, pluginName);
            return this;
        }

        public void Stop()
        {
            //Just tell the server you are going down.
            log.WriteLine("Plugin has been stopped!", LogLevel.Info, true, pluginName);
        }

        public void PlusExecute()
        {
            //The method of plus command.
            this.Init(Guid.NewGuid());
            log.WriteLine("1 + 1 = 2", LogLevel.Info);
        }
    }
}
