using Controller;

namespace ConsoleView
{
    public class Program
    { 
        public static void Main()
        {
            Data.Initialize();
            Visualisation.Initialize();
            Data.NextRace();

            Console.CursorVisible = false;

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
