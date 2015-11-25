using System;
using ServeKliyent_V2.Utils;
using ServeKliyent_V2.IO;

namespace ServeKliyent_V2
{
    public class Program
    {
        public static SettingManager settings = new SettingManager();
        public static Logging console = new Logging();
        public static bool keepAlive = true;
        static void Main(string[] args)
        {
            settings.loadSettings(Environment.CurrentDirectory + "/server.properties"); // Load settings from file
            settings.settings.Populate(); // Populate loaded settings into program

            console.BeginLog(); // Enable logging

            console.WriteLine("Settings loaded from ./server.properties", LogLevel.Info); //TODO: Make load path changable from args
            console.WriteLine("", LogLevel.Info);

            while (keepAlive)
            {
                console.Write("master#console:~$ ", LogLevel.Info, false);
                string command = console.ReadLine();

                CommandManagers.MasterConsole.HandleCommand(command, console);
            }

            console.StopLog();
        }
    }
}
