using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace Version_3._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void SetCulture(string language)
        {
            if (language == "English")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("En");
            }
            else if (language == "Français")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("Fr");
            }
        }
    }

}
