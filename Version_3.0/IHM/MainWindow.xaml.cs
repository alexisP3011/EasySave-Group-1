using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IHM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Client();
            Task.Run(() => Client.StartClient());
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is string workName)
            {
                Client.SendCommandToServer("Launch", workName);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is string workName)
            {
                Client.SendCommandToServer("Pause", workName);
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is string workName)
            {
                Client.SendCommandToServer("Stop", workName);
            }
        }

    }
}