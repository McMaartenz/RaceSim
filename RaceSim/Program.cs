using Controller;
using Grafische;
using System.Runtime.InteropServices;
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

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int HWND_HIDE = 0;
        const int HWND_SHOW = 5;

        private static bool HWND_SHOWN;

        public const VisualisationModes VISUALISATION_MODE = VisualisationModes.WPF_APP;

        private static bool canExit = false;

        public static void Main()
        {
            HWND_SHOWN = true;

            Data.Initialize();

            Thread WPFThread;
            if (VISUALISATION_MODE == VisualisationModes.CONSOLE)
            {
                Visualisation.Initialize();
            }
            else
            {
                bool done = false;
                WPFThread = new(() => {
                    Application app = new();
                    app.Run(new MainWindow(ref done));
                });

                WPFThread.SetApartmentState(ApartmentState.STA);
                WPFThread.Start();

                SpinWait.SpinUntil(() => done);

                MainWindow.WPFExit += (sender, e) => canExit = true;
                MainWindow.ToggleConsole += (sender, e) =>
                {
                    ShowWindow(GetConsoleWindow(), HWND_SHOWN ? HWND_HIDE : HWND_SHOW);
                    HWND_SHOWN = !HWND_SHOWN;
                };
            }

            Data.NextRace();
            Console.CursorVisible = false;

            SpinWait.SpinUntil(() => canExit);
            if (VISUALISATION_MODE == VisualisationModes.WPF_APP) WPFThread.Join();
        }
    }
}
