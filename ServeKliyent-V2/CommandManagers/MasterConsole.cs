using ServeKliyent_V2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServeKliyent_V2.CommandManagers
{
    public class MasterConsole
    {
        public static void HandleCommand(string command, Logging console)
        {
            List<string> commands = Regex.Matches(command, @"[\""].+?[\""]|[^ ]+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();

            if (commands[0] == "settings")
            {
                if (commands.Count > 1)
                {
                    if (commands[1] == "get")
                    {
                        if (commands[2] == "outputMode")
                        {
                            console.WriteLine("Value for outputMode: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);
                            console.WriteLine("Options for outputMode: ", LogLevel.Info);
                            console.Write("                  " + OutputMode.InfoOnly.ToString() + "", LogLevel.Info);
                            console.Write(", " + OutputMode.Essential.ToString() + "", LogLevel.Info);
                            console.Write(", " + OutputMode.Verbose.ToString() + "", LogLevel.Info);
                            console.Write(", " + OutputMode.Mute.ToString() + ".\n", LogLevel.Info);
                        }
                        else
                        {
                            console.WriteLine("Object '" + commands[2] + "' could not be found.", LogLevel.Info);
                        }
                    }
                    else if (commands[1] == "set")
                    {
                        if (commands[2] == "outputMode")
                        {
                            console.WriteLine("Value for outputMode was: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);

                            if (commands[3] == OutputMode.InfoOnly.ToString())
                                Program.settings.settings.outputMode = OutputMode.InfoOnly;
                            else if (commands[3] == OutputMode.Essential.ToString())
                                Program.settings.settings.outputMode = OutputMode.Essential;
                            else if (commands[3] == OutputMode.Verbose.ToString())
                                Program.settings.settings.outputMode = OutputMode.Verbose;
                            else if (commands[3] == OutputMode.Mute.ToString())
                                Program.settings.settings.outputMode = OutputMode.Mute;

                            console.WriteLine("Value for outputMode is now: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);

                            Program.settings.settings.Populate();
                            Program.settings.SaveSettings(Environment.CurrentDirectory + "/server.properties");
                        }
                        else
                        {
                            console.WriteLine("Object '" + commands[2] + "' could not be found.", LogLevel.Info);
                        }
                    }
                    else
                    {
                        console.WriteLine("The operator '" + commands[1] + "' is invalid.", LogLevel.Warning);
                    }
                }
                else
                {
                    console.WriteLine("Usage: settings [operator] [object] [value]\n" +
                 "                     Operators: get, set.\n" +
                 "                     Objects: outputMode\n"
                  , LogLevel.Warning);
                }
            }
            else if (commands[0] == "help")
            {
                throw new NotImplementedException();
            }
            else if (commands[0] == "clear")
            {
                Console.Clear();
            }
            else if (commands[0] == "exit")
            {
                Program.settings.SaveSettings(Environment.CurrentDirectory + "/server.properties");
                console.WriteLine("Settings saved to ./server.properties", LogLevel.Info); 
                //TODO: Make load path changable from Program->Main->args

                Program.console.StopLog();
                Environment.Exit(0);
            }
            else
            {
                console.WriteLine("The command '" + commands[0] + "' could not be found.", LogLevel.Warning);
            }
        }
    }
}
