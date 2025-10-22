using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _06_CopyFile
{
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<CopyProcessecInfo> processes;
        public string Source { get; set; }
        public string Destination { get; set; }
        public float Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        public IEnumerable<CopyProcessecInfo> Processes => processes;
        public ViewModel()
        {
            processes = [];
        }
        public void AddProcess(CopyProcessecInfo info)
        {
            processes.Add(info);
        }
    }
    [AddINotifyPropertyChangedInterface]
    class CopyProcessecInfo
    {
        public string Filename { get; set; }
        public float Percentage { get; set; }
    }
    public partial class MainWindow : Window
    {
        ViewModel model = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }
        private Task CopyFileAsync(string src, string dest, CopyProcessecInfo info)
        {
            return Task.Run(() =>
            {
                

                using FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read);
                using FileStream desStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
                byte[] buffer = new byte[1024 * 8];
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    desStream.Write(buffer, 0, bytes);

                    float percentage = desStream.Length / (srcStream.Length / 100);

                    model.Progress = percentage;
                    info.Percentage = percentage;


                } while (bytes > 0);


            });

        }

        private async void  Copy_Click(object sender, RoutedEventArgs e)
        {
            model.Destination = destTb.Text;
            model.Source = sourceTb.Text;

            string filename = Path.GetFileName(model.Source);
            string destFilename = Path.Combine(model.Destination, filename);

            CopyProcessecInfo info = new CopyProcessecInfo()
            {
                Filename = filename,
                Percentage = 0
            };

            model.AddProcess(info);
            await CopyFileAsync(model.Source, destFilename, info);
            MessageBox.Show("Completed!!!");
        }
    }
}