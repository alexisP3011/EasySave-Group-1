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
using Version_2._0.View.PopUp;

namespace Version_2._0.View.Button
{
    /// <summary>  
    /// Logique d'interaction pour DeleteButton.xaml  
    /// </summary>  
    public partial class DeleteButton : UserControl
    {
        public DeleteButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteButton_Click();
        }

        // Ajout de la méthode manquante DeleteButton_Click pour corriger l'erreur CS0103  
        private void DeleteButton_Click()
        {
            // Implémentez ici la logique de suppression ou affichez une fenêtre de confirmation  
            //var popup = new ConfirmationPopUp();
            //popup.Show();
        }
    }
}
