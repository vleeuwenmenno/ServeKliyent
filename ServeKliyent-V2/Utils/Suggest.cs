using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeKliyent_V2.Utils
{
    public class Suggest
    {
        public string command { get; set; }

        public string[] toSuggest { get; set; }

        public List<Suggest> childSuggests { get; set; }

        public Suggest()
        { }

        public Suggest(string command, string[] toSuggest)
        {
            this.command = command;
            this.toSuggest = toSuggest;
        }
    }
}
