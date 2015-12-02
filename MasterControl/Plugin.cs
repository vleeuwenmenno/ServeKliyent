using ServeKliyent_V2.CommandManagers;
using ServeKliyent_V2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MasterControl
{
    public class Plugin : ServeKliyent_V2.Plugin.Plugin
    {
        public static bool masterServerStatus = false;
        public ServeKliyent_V2.Plugin.Plugin Start()
        {
            //Initalize the plugin at the pluginManager
            Init(Guid.NewGuid());
            pluginName = "MasterControl";

            //Register commands
            Command cmd = new Command(this, pluginName);

            //Define the command info
            cmd.command = "master";
            cmd.method = "MasterControl";
            cmd.usage = "Control the master server.";

            //Finally register the command in the commandManager
            commandMan.RegisterCommand(cmd);

            //Start serving
            StartServe();

            //Tell the user that we are loaded
            log.WriteLine("MasterControl is loaded!", LogLevel.Info, true, pluginName);
            return this;
        }

        public void MasterControl(string[] args)
        {
            if (args.Length > 1)
            {
                if (args[1] == "status")
                {
                    log.WriteLine("Master server is currently " + (masterServerStatus == true ? "running." : "not running."), LogLevel.Info);
                }
                else if (args[1] == "start")
                {
                    log.WriteLine("Starting master server...", LogLevel.Info);
                    StartServe();
                }
                else if (args[1] == "stop")
                {
                    log.WriteLine("Stopping master server...", LogLevel.Info);
                    StopServe();
                }
                else 
                {
                    log.WriteLine("Invalid parameters!", LogLevel.Warning);
                }
            }
        }

        public void Stop()
        {
            //Tell the server we are going down.
            if (masterServerStatus)
                StopServe();

            log.WriteLine("MasterControl is unloaded.", LogLevel.Info, true, pluginName);
        }

        public void StopServe()
        {
            if (masterServerStatus)
            {
                masterServerStatus = false;
                _listener.Stop();
                _listener = null;

                GC.Collect();
                log.WriteLine("Master server is now down.", LogLevel.Info);
            }
            else
            {
                log.WriteLine("Master server is already stopped!", LogLevel.Warning);
            }
        }

        public void StartServe()
        {
            if (!masterServerStatus)
            {
                masterServerStatus = true;
                StartServer();

                log.WriteLine("Master server is running...", LogLevel.Info);
            }
            else
            {
                log.WriteLine("Master server is already running!", LogLevel.Warning);
            }
        }

        private static TcpListener _listener;

        public static void StartServer()
        {
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8888);
            _listener = new TcpListener(ipLocal);
            _listener.Start();

            WaitForClientConnect();
        }

        private static void WaitForClientConnect()
        {
            if (masterServerStatus)
            {
                object obj = new object();
                _listener.BeginAcceptTcpClient(new System.AsyncCallback(OnClientConnect), obj);
            }
        }

        private static void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = _listener.EndAcceptTcpClient(asyn);
                HandleClientRequest clientReq = new HandleClientRequest(clientSocket);
                clientReq.StartClient();
            }
            catch (Exception se)
            {
                if (masterServerStatus)
                    throw;
            }

            WaitForClientConnect();
        }
    }
}
