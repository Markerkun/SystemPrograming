using System.Collections.ObjectModel;
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
using Microsoft.Win32;

namespace _07_multyparalelthreads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ViewModel model = new ViewModel();
        public MainWindow()
        {
            InitializeComponent(); 
            model.AddProcess(new CopyProcessecInfo() { Percentage = 10, Filename = "test1.txt" });
            model.AddProcess(new CopyProcessecInfo() { Percentage = 10, Filename = "test1.txt" });
            model.Source = sourceTb.Text = @"C:\Users\helen\Downloads\1GB.bin";
            model.Destination = DestTb.Text = @"C:\Users\helen\Desktop\Test";
            model.Progress = 0;
            this.DataContext = model;

        }

            

        private void SourceBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                //MessageBox.Show(fileDialog.FileName);
                model.Source = sourceTb.Text = fileDialog.FileName;
            }
        }

        private void DestBtn(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            //CommonFileDialogResult res =  dialog.ShowDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = DestTb.Text = dialog.FileName;
            }
        }

        private async void CopyBtn(object sender, RoutedEventArgs e)
        {
            //C:\Users\helen\Desktop\Testtest.bin
            // Path.Combine(Source, Destination);
            string filename = Path.GetFileName(model.Source);
            string destFilename = Path.Combine(model.Destination, filename);
            //add to list item
            CopyProcessecInfo info = new CopyProcessecInfo()
            {
                Filename = filename,
                Percentage = 0
            };

            model.AddProcess(info);
            await CopyFileAsync(model.Source, destFilename, info);
            MessageBox.Show("Completed!!!");

        }

        private Task CopyFileAsync(string src, string dest, CopyProcessecInfo info)
        {
            return Task.Run(() =>
            {
                //File.Copy(s, d, true);
                //2 - FileStream
                using FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read);
                using FileStream desStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
                byte[] buffer = new byte[1024 * 8];//8Kb
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    desStream.Write(buffer, 0, bytes);

                    float percentage = desStream.Length / (srcStream.Length / 100);
                    //progress.Value = percentage;    
                    model.Progress = percentage;
                    info.Percentage = percentage;


                } while (bytes > 0);


            });

        }
    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<CopyProcessecInfo> processes;
        public string Source { get; set; }
        public string Destination { get; set; }
        public float Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        public IEnumerable<CopyProcessecInfo> Processes => processes;//get - readonly
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
}
}