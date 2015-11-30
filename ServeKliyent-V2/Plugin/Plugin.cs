using System;
using System.Reflection;

namespace ServeKliyent_V2.Plugin
{
    public class Plugin
    {
        public Assembly assembly { get; set; }

        public Type[] types { get; set; }

        public MethodInfo[] methods { get; set; }

        /// <summary>
        /// Execute a method from a plugin. When Class of type int is left empty it will search the first upcoming method called method of type string
        /// </summary>
        /// <param name="method">The method you want to execute</param>
        /// <param name="Class">From which class this method is child to. When left empty it will search the first upcoming method called method of type string</param>
        public void Execute(string method, int Class = -1)
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

                object instance = Activator.CreateInstance(types[Class]);

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
                    methods[startMethod].Invoke(instance, null);
                else if (!classOk)
                {
                    Program.console.WriteLine("Error failed to execute requested method, class with ID '" + Class + "' could not be found!", Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);
                }
                else
                {
                    Program.console.WriteLine("Error failed to execute requested method, method '" + method + "' could not be found!", Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);
                }
            }
            catch (Exception ex)
            {
                    Program.console.WriteLine("Error failed to execute requested method, " + ex.Message, Utils.LogLevel.Error, true, assembly.FullName.Split(',')[0]);
            }
        }
    }
}
