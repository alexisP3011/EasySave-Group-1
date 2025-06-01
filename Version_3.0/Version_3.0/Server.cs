using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime;
using System.Text.Json;

namespace Version_3._0
{
    internal class Server
    {
        static IPEndPoint clientEndPoint;
        static string JsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "works.json");

        public static Socket StartServer()
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 1448);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverEndPoint);

            serverSocket.Listen(1);

            MessageBox.Show("Server is listening...");
            return serverSocket;
        }

        public static void SendJobsFromFile(Socket clientSocket, string jsonFilePath)
        {

            try
            {
                while (true)
                {
                    if (!clientSocket.Connected)
                        break;

                    string json = File.ReadAllText(jsonFilePath);
                    byte[] buffer = Encoding.UTF8.GetBytes(json + "\n");
                    clientSocket.Send(buffer);

                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex){}
        }

        public static Socket AcceptConnection(Socket serverSocket)
        {
            Socket clientSocket = serverSocket.Accept();

            clientEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;

            MessageBox.Show(string.Format("Connected to address {0} on port {1}", clientEndPoint.Address, clientEndPoint.Port));
            return clientSocket;
        }

        private static void Disconnect(Socket serverSocket)
        {
            MessageBox.Show(string.Format("Disconnected from {0}", clientEndPoint.Address));
            serverSocket.Close();
        }

        public static void Start()
        {
            Socket serverSocket = StartServer();
            Socket clientSocket = AcceptConnection(serverSocket);
            SendJobsFromFile(clientSocket, JsonPath);
            Disconnect(serverSocket);
        }
    }
}
