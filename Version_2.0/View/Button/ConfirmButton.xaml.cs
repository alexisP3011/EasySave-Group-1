using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Version_2._0.View.Button
{
    /// <summary>
    /// Logique d'interaction pour ConfirmButton.xaml
    /// </summary>
    public partial class ConfirmButton : UserControl
    {
        public event RoutedEventHandler Click;

        public ConfirmButton()
        {
            InitializeComponent();
        }

        private void InnerButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e); // We propagate the click
        }


        public void InternalButton_Click(object sender, RoutedEventArgs e)
        {
            // We propagate the Click event to the control's users
            //Click?.Invoke(this, e);


        }
    }

}
