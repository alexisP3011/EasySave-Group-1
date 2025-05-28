using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IHM
{
    class Client
    {
        public static void ConnectToServer()
        {
            try
            {
                // Adresse IP et port du serveur
                IPAddress serverIp = IPAddress.Parse("127.0.0.1"); // ou l’IP du serveur distant
                int port = 1448;

                IPEndPoint serverEndPoint = new IPEndPoint(serverIp, port);

                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(serverEndPoint);

                MessageBox.Show("Connecté au serveur !");

                // Pas d'action pour l'instant. On reste connecté.
            }
            catch (SocketException ex)
            {
                Console.WriteLine("❌ Erreur de connexion : " + ex.Message);
            }
        }
    }
}
