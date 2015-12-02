using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer_Client
{
    public class TCPClient
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;

        public static void Main(string[] args)
        {
            TCPClient tcp = new TCPClient();
            tcp.Start(args);
        }

        public void Start(string[] args)
        {
            if (args.Length > 0)
                clientSocket.Connect(args[0], 8888);
            else
                clientSocket.Connect(Console.ReadLine(), 8888);
        }

        public void SendData(string dataTosend)
        {
            if (string.IsNullOrEmpty(dataTosend))
                return;
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dataTosend);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void CloseConnection()
        {
            clientSocket.Close();
        }
        public string ReceiveData()
        {
            StringBuilder message = new StringBuilder();
            NetworkStream serverStream = clientSocket.GetStream();
            serverStream.ReadTimeout = 100;
            //the loop should continue until no dataavailable to read and message string is filled.
            //if data is not available and message is empty then the loop should continue, until
            //data is available and message is filled.
            while (true)
            {
                if (serverStream.DataAvailable)
                {
                    int read = serverStream.ReadByte();
                    if (read > 0)
                        message.Append((char)read);
                    else
                        break;
                }
                else if (message.ToString().Length > 0)
                    break;
            }
            return message.ToString();
        }
    }
}
