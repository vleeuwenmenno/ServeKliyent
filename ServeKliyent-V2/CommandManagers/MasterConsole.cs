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

            Commands.command(commands);
        }
    }
}
