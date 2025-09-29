using System.Diagnostics;
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

namespace _01_TaskManeger
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = Process.GetProcesses();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (grid.SelectedItem as Process).Kill();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process process = (grid.SelectedItem as Process);
            string message = " ";
            message += process.MachineName;

            MessageBox.Show(message, "Process info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
}