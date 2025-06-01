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
using System.Diagnostics;
using Version_3._0.View.Popup;
using static Version_3._0.MainWindow;
using System.Windows.Threading;

namespace Version_3._0
{
    internal class Server
    {
        private static Socket serverSocket;
        private static Socket clientSocket;
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

        public static void ListenCommand(Socket clientSocket)
        {
            while (true)
            {
                byte[] receivedBuffer = new byte[4096];
                int received = clientSocket.Receive(receivedBuffer);

                if (received > 0)
                {
                    string json = Encoding.UTF8.GetString(receivedBuffer, 0, received).Trim();

                    try
                    {
                        var command = JsonSerializer.Deserialize<Command>(json);

                        if (command != null && command.Action == "Launch")
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var mainWindow = MainWindow.Instance;

                                foreach (var work in mainWindow.Works)
                                {
                                    work.IsSelected = (work.Name == command.WorkName);
                                }

                                mainWindow.LaunchButton_Click(null, null);
                            });
                        }
                        if (command != null && command.Action == "Stop")
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var mainWindow = MainWindow.Instance;

                                foreach (var work in mainWindow.Works)
                                {
                                    work.IsSelected = (work.Name == command.WorkName);
                                }

                                mainWindow.StopButton_Click(null, null);
                            });
                        }
                        if (command != null && command.Action == "Pause")
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var mainWindow = MainWindow.Instance;

                                foreach (var work in mainWindow.Works)
                                {
                                    work.IsSelected = (work.Name == command.WorkName);
                                }

                                mainWindow.PauseButton_Click(null, null);
                            });
                        }
                    }
                    catch (Exception ex){}
                }                    

                
            }
        }

        public static void SendJobsFromFile(Socket clientSocket, string jsonFilePath)
        {

            try
            {
                while (true)
                {
                    if (!clientSocket.Connected)
                        break;
                    if (File.Exists(jsonFilePath))
                    {
                        string jsonFromFile = File.ReadAllText(jsonFilePath);
                        List<Work> jobs = JsonSerializer.Deserialize<List<Work>>(jsonFromFile);

                        foreach (var job in jobs)
                        {
                            job.Progress = RealTimeLog.getInstance().Progression(job.Source, job.Target);
                        }

                        string enrichedJson = JsonSerializer.Serialize(jobs);
                        byte[] buffer = Encoding.UTF8.GetBytes(enrichedJson);
                        clientSocket.Send(buffer);

                        Thread.Sleep(3000);
                    }

                }
            }
            catch (Exception ex) { }
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
            serverSocket = StartServer();
            clientSocket = AcceptConnection(serverSocket);
            Task.Run(() => ListenCommand(clientSocket));
            SendJobsFromFile(clientSocket, JsonPath);
            Disconnect(serverSocket);
        }
    }

    public class Command
    {
        public string Action { get; set; }
        public string WorkName { get; set; }
    }
}
