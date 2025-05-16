using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Version_2._0.View;
using Version_2._0.View.PopUp;
using System.ComponentModel;

namespace Version_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Work work = new Work
            //{
            //    Name = "Test",
            //    Source = @"C:\Users\andre\Desktop\test",
            //    Target = @"C:\Users\andre\Desktop\test2",
            //    Type = "copy",
            //    State = "inactive"
            //};

            //this.DataContext = work;
        }
    }




    public class Work : INotifyPropertyChanged
        {
            private string name;
            private string source;
            private string target;
            private string type;
            private string state;

            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }

            public string Source
            {
                get => source;
                set
                {
                    source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }

            public string Target
            {
                get => target;
                set
                {
                    target = value;
                    OnPropertyChanged(nameof(Target));
                }
            }

            public string Type
            {
                get => type;
                set
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }

            public string State
            {
                get => state;
                set
                {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }

            public Work()
            {
                Name = "";
                Source = "";
                Target = "";
                Type = "";
                State = "inactive";
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

}