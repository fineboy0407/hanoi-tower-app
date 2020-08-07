using System.Linq;
using System.Windows;

namespace WpfHanoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DiscNumberEntered(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(Discs.Text, out var _) == false)
            {
                if(Discs.Text.Length > 0)
                {
                    var text = Discs.Text.Substring(0, Discs.Text.Count() - 1);
                    Discs.Text = text;
                    Discs.CaretIndex = Discs.Text.Count();
                }
            }
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            new Hanoi(int.Parse(Discs.Text)).Show();
            Close();
        }
    }
}
