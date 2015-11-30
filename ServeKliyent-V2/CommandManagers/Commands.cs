﻿using ServeKliyent_V2.Utils;
using System;
using System.Collections.Generic;

namespace ServeKliyent_V2.CommandManagers
{
    public class Commands
    {
        public Commands()
        {
            commandsRegistered = new List<Command>();
        }

        public List<Command> commandsRegistered { get; internal set; }

        public static void command(List<string> commands)
        {
            if (commands[0] == "settings")
            {
                if (commands.Count > 1)
                {
                    if (commands[1] == "get")
                    {
                        if (commands[2] == "outputMode")
                        {
                            Program.console.WriteLine("Value for outputMode: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);
                            Program.console.WriteLine("Options for outputMode: ", LogLevel.Info);
                            Program.console.Write("                  " + OutputMode.InfoOnly.ToString() + "", LogLevel.Info);
                            Program.console.Write(", " + OutputMode.Essential.ToString() + "", LogLevel.Info);
                            Program.console.Write(", " + OutputMode.Verbose.ToString() + "", LogLevel.Info);
                            Program.console.Write(", " + OutputMode.Mute.ToString() + ".\n", LogLevel.Info);
                        }
                        else
                        {
                            Program.console.WriteLine("Object '" + commands[2] + "' could not be found.", LogLevel.Info);
                        }
                    }
                    else if (commands[1] == "set")
                    {
                        if (commands[2] == "outputMode")
                        {
                            Program.console.WriteLine("Value for outputMode was: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);

                            if (commands[3] == OutputMode.InfoOnly.ToString())
                                Program.settings.settings.outputMode = OutputMode.InfoOnly;
                            else if (commands[3] == OutputMode.Essential.ToString())
                                Program.settings.settings.outputMode = OutputMode.Essential;
                            else if (commands[3] == OutputMode.Verbose.ToString())
                                Program.settings.settings.outputMode = OutputMode.Verbose;
                            else if (commands[3] == OutputMode.Mute.ToString())
                                Program.settings.settings.outputMode = OutputMode.Mute;

                            Program.console.WriteLine("Value for outputMode is now: " + Program.settings.settings.outputMode.ToString(), LogLevel.Info);

                            Program.settings.settings.Populate();
                            Program.settings.SaveSettings(Environment.CurrentDirectory + "/server.properties");
                        }
                        else
                        {
                            Program.console.WriteLine("Object '" + commands[2] + "' could not be found.", LogLevel.Info);
                        }
                    }
                    else
                    {
                        Program.console.WriteLine("The operator '" + commands[1] + "' is invalid.", LogLevel.Warning);
                    }
                }
                else
                {
                    Program.console.WriteLine("Usage: settings [operator] [object] [value]\n" +
                 "                     Operators: get, set.\n" +
                 "                     Objects: outputMode\n"
                  , LogLevel.Warning);
                }
            }
            else if (commands[0] == "help")
            {
                Program.console.WriteLine("Usage: [command] [parameters] [..] [..] ....\n" +
                 "                     Internal Commands:\n" +
                 "                     exit - Save and exit the server.\n" +
                 "                     clear - Clear the console screen.\n" +
                 "                     help - Show this help screen.\n" +
                 "                     settings - Change the server settings\n", LogLevel.Info);

                string helpString = "";

                foreach (Command cmd in Program.commandMan.commandsRegistered)
                {
                    helpString += "                     " + cmd.command + " - " + cmd.usage + "\n";
                }

                Program.console.WriteLine("External Commands:\n" + helpString +
                 "\n                     For more usage info type help [command] //TODO", LogLevel.Info); //TODO: Make a help [command] system!
            }
            else if (commands[0] == "clear")
            {
                Console.Clear();
            }
            else if (commands[0] == "exit")
            {
                Program.settings.SaveSettings(Environment.CurrentDirectory + "/server.properties");
                Program.console.WriteLine("Settings saved to ./server.properties", LogLevel.Info);
                //TODO: Make load path changable from Program->Main->args

                Program.console.StopLog();
                Environment.Exit(0);
            }
            else
            {
                foreach (Command c in Program.commandMan.commandsRegistered)
                {
                    if (c.command == commands[0])
                    {
                        
                    }
                }

                Program.console.WriteLine("The command '" + commands[0] + "' could not be found.", LogLevel.Warning);
            }
        }

        public void RegisterCommand(Command cmd)
        {
            bool dupe = false;

            foreach (Command cm in commandsRegistered)
            {
                if (cm.command == cmd.command)
                    dupe = true;
            }

            if (!dupe)
            {
                commandsRegistered.Add(cmd);
                Program.console.WriteLine("Command '" + cmd.command + "' successfully registered.", LogLevel.Debug, true, cmd.parentName);
            }
            else
            {
                Program.console.WriteLine("Failed to register command '" + cmd.command + "' command string already in use!", LogLevel.Error, true, cmd.parentName);
            }
        }

        public void UnregisterCommand(Command cmd)
        {
            bool exist = false;

            foreach (Command cm in commandsRegistered)
            {
                if (cm == cmd)
                    exist = true;
            }

            if (exist)
            {
                commandsRegistered.Remove(cmd);
                Program.console.WriteLine("Command '" + cmd.command + "' successfully unregistered.", LogLevel.Debug, true, cmd.parentName);
            }
            else
            {
                Program.console.WriteLine("Failed to unregister command '" + cmd.command + "' command not found!", LogLevel.Error, true, cmd.parentName);
            }
        }
    }
}