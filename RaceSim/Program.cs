using Controller;
using Grafische;
using System.Windows;

namespace ConsoleView
{
    public class Program
    {
        public enum VisualisationModes
        {
            CONSOLE,
            WPF_APP
        }

        public const VisualisationModes VISUALISATION_MODE = VisualisationModes.WPF_APP;

        public static void Main()
        {

            Data.Initialize();
            
            if (VISUALISATION_MODE == VisualisationModes.CONSOLE)
            {
                Visualisation.Initialize();
            }
            else
            {
                bool done = false;
                Thread thread = new(() => {
                    Application app = new();
                    app.Run(new MainWindow(ref done));
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                SpinWait.SpinUntil(() => done);
            }

            Data.NextRace();

            Console.CursorVisible = false;

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
