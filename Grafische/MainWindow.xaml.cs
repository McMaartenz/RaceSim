using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public delegate void WPFApplicationExit(object? sender, EventArgs e);
        public static event WPFApplicationExit WPFExit;

        public delegate void ToggleConsoleView(object? sender, EventArgs e);
        public static event ToggleConsoleView ToggleConsole;

        private SchermA schermA;
        private SchermB schermB;

        private static object changingTrack = new();

        public MainWindow(ref bool done)
        {
            InitializeComponent();
            Closing += (sender, e) => WPFExit(sender, e);

            Data.RaceChanged += Track_RaceChanged;

            schermA = new();
            schermB = new();

            done = true;
        }

        private void Track_RaceChanged(object? sender, RaceChangedEventArgs e)
        {
            Console.WriteLine("Track has changed");

            lock (changingTrack)
            {
                (int w, int h) = ImageManager.CalculateTrackPixelDimensions(e.race.Track);
                ImageManager.FlushCache();

                ImageManager.trackWidth = w;
                ImageManager.trackHeight = h;
                
                ImageManager.GetEmptyTrackBitmap(e.race.Track, e.race);
            }
            e.race.DriversChanged += Track_DriversChanged;
        }

        private void Track_DriversChanged(object? sender, DriversChangedEventArgs e)
        {
            RenderThread(() =>
            {
                lock (changingTrack)
                {
                    BitmapSource src = ImageManager.ToBitmapSource(Visualisation.DrawTrack(e.track, Data.CurrentRace));
                    trackImage.Source = src;
                }
            });
        }

        private void RenderThread(Action action)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                action
            );
        }

        // Send events to Program Console
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            WPFExit?.Invoke(sender, e);
            Application.Current.Shutdown();
        }
        
        private void Toggle_Console_View(object sender, RoutedEventArgs e)
        {
            ToggleConsole?.Invoke(sender, e);
        }

        private void Toggle_SchermA_View(object sender, RoutedEventArgs e)
        {
            if (schermA.IsVisible)
            {
                schermA.Hide();
            }
            else
            {
                schermA.Show();
            }
        }

        private void Toggle_SchermB_View(object sender, RoutedEventArgs e)
        {
            if (schermB.IsVisible)
            {
                schermB.Hide();
            }
            else
            {
                schermB.Show();
            }
        }
    }
}
