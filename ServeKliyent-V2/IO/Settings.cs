using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServeKliyent_V2.Utils;

namespace ServeKliyent_V2.IO
{
    [Serializable]
    public class Settings
    {
        public OutputMode outputMode { get; set; }

        public void Populate()
        {
            Program.console.outputMode = outputMode;
        }
    }
}
