using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Grafische
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(ref bool done)
        {
            InitializeComponent();

            Data.RaceChanged += Track_RaceChanged;
            done = true;
        }
        private void Track_RaceChanged(object? sender, RaceChangedEventArgs e)
        {
            Console.WriteLine("Track has changed");

            (int w, int h) = ImageManager.CalculateTrackPixelDimensions(Controller.Data.CurrentRace.Track);

            ImageManager.trackWidth = w;
            ImageManager.trackHeight = h;

            e.race.DriversChanged += Track_DriversChanged;
        }

        private void Track_DriversChanged(object? sender, DriversChangedEventArgs e)
        {
            RenderThread(() =>
            {
                BitmapSource src = ImageManager.ToBitmapSource(Visualisation.DrawTrack(e.track, Data.CurrentRace));
                trackImage.Source = src;
            });
        }

        private void RenderThread(Action action)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                action
            );
        }
    }
}
