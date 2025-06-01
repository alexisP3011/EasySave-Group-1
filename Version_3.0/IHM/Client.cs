using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IHM
{
    class Client
    { 
        public static ObservableCollection<Work> Tasks { get; set; } = new ObservableCollection<Work>();

        private static void Disconnect(Socket serverSocket)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }
        private static Socket ConnectToServer()
        {
            IPAddress serverIp = IPAddress.Parse("127.0.0.1");
            int port = 1448;

            IPEndPoint serverEndPoint = new IPEndPoint(serverIp, port);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(serverEndPoint);

            MessageBox.Show("Connecté au serveur !");

            return clientSocket;
        }

        private static void ListenToServer(Socket socket)
        {
            while (true)
            {
                byte[] dataBuffer = new byte[4096];
                int receivedDataLength = socket.Receive(dataBuffer);
                string json = Encoding.UTF8.GetString(dataBuffer, 0, receivedDataLength).Trim();

                try
                {
                    var receivedTasks = JsonSerializer.Deserialize<List<Work>>(json);

                    if (receivedTasks != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var namesFromJson = new HashSet<string>(receivedTasks.Select(t => t.Name));
                            var toRemove = Client.Tasks.Where(t => !namesFromJson.Contains(t.Name)).ToList();

                            foreach (var item in toRemove)
                            {
                                Client.Tasks.Remove(item);
                            }

                            foreach (var received in receivedTasks)
                            {
                                var existing = Client.Tasks.FirstOrDefault(t => t.Name == received.Name);
                                if (existing == null)
                                {
                                    Client.Tasks.Add(received);
                                }
                                else
                                {
                                    existing.Source = received.Source;
                                    existing.Target = received.Target;
                                    existing.Type = received.Type;
                                    existing.State = received.State;
                                    existing.Progress = received.Progress;
                                }
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur JSON : " + ex.Message);
                }
            }
        }

        public static void StartClient()
        {
            Socket socket = ConnectToServer();
            ListenToServer(socket);
            Disconnect(socket);
        }
    }
}
public class Work : INotifyPropertyChanged
{
    private string name, source, target, type, state;
    private double progress;

    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(nameof(Name)); }
    }

    public string Source
    {
        get => source;
        set { source = value; OnPropertyChanged(nameof(Source)); }
    }

    public string Target
    {
        get => target;
        set { target = value; OnPropertyChanged(nameof(Target)); }
    }

    public string Type
    {
        get => type;
        set { type = value; OnPropertyChanged(nameof(Type)); }
    }

    public string State
    {
        get => state;
        set { state = value; OnPropertyChanged(nameof(State)); }
    }

    public double Progress
    {
        get => progress;
        set { progress = value; OnPropertyChanged(nameof(Progress)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
