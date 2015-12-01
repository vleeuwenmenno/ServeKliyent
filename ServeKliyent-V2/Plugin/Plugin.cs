using ServeKliyent_V2.CommandManagers;
using ServeKliyent_V2.Utils;
using System;
using System.Reflection;

namespace ServeKliyent_V2.Plugin
{
    public class Plugin
    {
        public string pluginName { get; set; }

        public Logging log { get; set; }

        public Commands commandMan { get; set; }

        public Assembly assembly { get; set; }

        public Type[] types { get; set; }

        public MethodInfo[] methods { get; set; }

        public string pluginId { get; set; }

        public object pluginInstance { get; set; }

        public void Init(Guid pluginId)
        {
            assembly = Assembly.GetCallingAssembly();

            types = assembly.GetExportedTypes();

            int mainType = 0;

            foreach (Type t in types)
            {
                if (types[mainType].Name == "Plugin")
                    break;

                mainType++;
            }

            methods = types[mainType].GetMethods();

            log = Program.console;
            commandMan = Program.commandMan;

            this.pluginId = pluginId.ToString();
        }

        /// <summary>
        /// Execute a method from a plugin. When Class of type int is left empty it will search the first upcoming method called method of type string
        /// </summary>
        /// <param name="method">The method you want to execute</param>
        /// <param name="Class">From which class this method is child to. When left empty it will search the first upcoming method called method of type string</param>
        public object Execute(string method, int Class = -1)
        {
            try
            {
                bool classOk = false;

                if (Class != -1)
                {
                    if (Class <= types.Length)
                        classOk = true;
                }
                else
                {
                    while (!classOk)
                    {
                        if (Class < types.Length - 1)
                            Class++;
                        else
                            break;

                        object i = Activator.CreateInstance(types[Class]);

                        int sM = 0;
                        bool f = false;

                        foreach (MethodInfo mi in methods)
                        {
                            if (mi.Name == method)
                            {
                                f = true;
                                break;
                            }

                            sM++;
                        }

                        if (f)
                            classOk = true;
                    }
                }

                object instance = null;

                if (pluginInstance == null)
                    instance = Activator.CreateInstance(types[Class]);
                else
                    instance = pluginInstance;

                int startMethod = 0;
                bool found = false;

                foreach (MethodInfo mi in methods)
                {
                    if (mi.Name == method)
                    {
                        found = true;
                        break;
                    }

                    startMethod++;
                }

                if (found && classOk)
                {
                    object returnVal = methods[startMethod].Invoke(instance, null);
                    return returnVal;
                }
                else if (!classOk)
                {
                    Program.console.WriteLine("Error details: " + Class + " class index. Method: " + method, LogLevel.Debug);
                    Program.console.WriteLine("Error failed to execute requested method, class with ID 's" + Class + "' could not be found!", Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);

                    return null;
                }
                else
                {
                    Program.console.WriteLine("Error details: " + Class + " class index. Instance: ", LogLevel.Debug);
                    Program.console.WriteLine("Error failed to execute requested method, method '" + method + "' could not be found!", Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Program.console.WriteLine("Error details: " + ex.Message + "\n" + ex + "\n", LogLevel.Debug);
                Program.console.WriteLine("Error failed to execute requested method, " + ex.Message, Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);
                return null;
            }
        }
    }
}
