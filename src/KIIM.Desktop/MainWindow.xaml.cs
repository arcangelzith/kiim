using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace KIIM.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] paths;

        private void scanImages ()
        {
            Regex pattern = new Regex(@"\.(jpg|gif|png)$", RegexOptions.IgnoreCase);

            const string directory = "C:\\Users\\arcan\\OneDrive\\Imágenes\\Auto ayuda";

            string[] paths = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                                .Where(path => pattern.IsMatch(path))
                                .Select(path => Path.GetRelativePath(directory, path))
                                .ToArray();

            this.paths = paths;
        }

        public MainWindow ()
        {
            InitializeComponent();

            // this.Visibility = Visibility.Hidden;

            this.scanImages();
        }

        private void imageContainer_Loaded(object sender, RoutedEventArgs e)
        {
            // Set image size as window size


        }
    }
}
