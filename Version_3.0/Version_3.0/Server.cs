using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Version_3._0
{
    internal class Server
    {
        static IPEndPoint clientEndPoint;
        public static Socket StartServer()
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 1448);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverEndPoint);

            serverSocket.Listen(1);

            MessageBox.Show("Server is listening...");
            return serverSocket;
        }

        public static Socket AcceptConnection(Socket serverSocket)
        {
            Socket clientSocket = serverSocket.Accept();

            clientEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;

            MessageBox.Show(string.Format("Connected to address {0} on port {1}", clientEndPoint.Address, clientEndPoint.Port));
            return clientSocket;
        }

        private static void ListenToClient(Socket clientSocket)
        {
            
        }

        private static void Disconnect(Socket serverSocket)
        {
            MessageBox.Show(string.Format("Disconnected from {0}", clientEndPoint.Address));

            serverSocket.Close();

            Console.ReadLine();
        }

        public static void Start()
        {
            Socket serverSocket = StartServer();
            Socket clientSocket = AcceptConnection(serverSocket);
            ListenToClient(clientSocket);
            Disconnect(serverSocket);
        }
    }
}
