using ServeKliyent_V2.Utils;
using ServeKliyent_V2.CommandManagers;

namespace PlusCommands
{
    public class Plugin : ServeKliyent_V2.Plugin.Plugin
    {
        public string pluginName = "PlusCommands";
        public Logging log;
        public Commands commandMan;
        
        public void Start()
        {
            log = ServeKliyent_V2.Program.console; //Get te console for logging.
            commandMan = ServeKliyent_V2.Program.commandMan;

            //Register commands
            Command cmd = new Command(this, pluginName);

            cmd.command = "plus";
            cmd.method = "PlusExecute";
            cmd.usage = "Display 1 + 1 :P";

            commandMan.RegisterCommand(cmd);

            log.WriteLine("Plugin has been loaded!", LogLevel.Info, true, pluginName);
        }

        public void PlusExecute()
        {

        }
    }
}
