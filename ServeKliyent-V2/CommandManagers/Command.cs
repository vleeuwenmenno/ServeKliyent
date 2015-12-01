namespace ServeKliyent_V2.CommandManagers
{
    public class Command
    {
        public string command { get; set; }

        public string method { get; set; }

        public string usage { get; set; }

        public Plugin.Plugin parent { get; set; }

        public string parentName { get; }

        public Command(Plugin.Plugin plug, string parentName)
        {
            this.parent = plug;
            this.parentName = parentName;
        }
    }
}
