using Controller;

namespace ConsoleView
{
    public class Program
    { 
        public static void Main()
        {
            Data.Initialize();
            Data.NextRace();
            Visualisation.Initialize();

            Console.CursorVisible = false;

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
