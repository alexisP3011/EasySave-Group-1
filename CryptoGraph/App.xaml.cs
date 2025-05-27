using System.Configuration;
using System.Data;
using System.Windows;

namespace CryptoGraph
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "CryptoGraphAppSingleton";

            bool createdNew;
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Une instance de CryptoSoft est déjà en cours d'exécution.",
                                "Instance existante",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }
    }

}
