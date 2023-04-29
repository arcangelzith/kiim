using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private string ImageDirectory
        {
            get
            {
                return Properties.Settings.Default.Directory;
            }
        }

        private string[] imagePaths = { };

        private int pathIndex;

        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(3),
        };

        private void scanImages ()
        {
            Regex pattern = new Regex(@"\.(?:jpg|png|gif)$", RegexOptions.IgnoreCase);

            string[] paths = System.IO.Directory.GetFiles(this.ImageDirectory, "*.*", SearchOption.AllDirectories)
                                .Where(path => pattern.IsMatch(path))
                                .Select(path => Path.GetRelativePath(this.ImageDirectory, path))
                                .ToArray();

            Random random = new Random();

            this.imagePaths = paths.OrderBy(path => random.Next()).ToArray();

            this.pathIndex = 0;
        }

        public MainWindow ()
        {
            InitializeComponent();

            this.unloadImage();
            this.scanImages();

            this.timer.Tick += Timer_Tick; ;

            this.timer.Start();
        }

        private void Timer_Tick (object? sender, EventArgs e)
        {
            this.loadNextImage();
        }

        private void unloadImage ()
        {
            this.imageBorder.BeginInit();

            this.imageBorder.Width = 0;
            this.imageBorder.Height = 0;

            this.imageBorder.Background = null;

            this.imageBorder.Style = null;

            this.imageBorder.EndInit();

            this.Visibility = Visibility.Hidden;
            this.imageBorder.Visibility = Visibility.Hidden;
        }

        private void loadNextImage ()
        {
            if (this.imagePaths.Length == 0)
                return;

            this.pathIndex++;

            if (this.pathIndex == this.imagePaths.Length)
                this.pathIndex = 0;

            string relativePath = this.imagePaths[this.pathIndex];
            string absolutePath = Path.Join(this.ImageDirectory, relativePath);

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            
            bitmap.UriSource = new Uri(absolutePath);

            bitmap.EndInit();

            if (bitmap != null)
            {
                this.imageBorder.BeginInit();

                this.imageBorder.Width = bitmap.PixelWidth;
                this.imageBorder.Height = bitmap.PixelHeight;

                this.imageBorder.Background = new ImageBrush(bitmap)
                {
                    Stretch = Stretch.Fill,
                };

                this.imageBorder.Style = (Style)this.FindResource("FadeInAnimation");

                this.imageBorder.EndInit();
            }

            this.Visibility = Visibility.Visible;
            this.imageBorder.Visibility = Visibility.Visible;
        }

        private void Window_MouseDown (object sender, MouseButtonEventArgs e)
        {
            this.unloadImage();
        }
    }
}
