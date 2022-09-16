using Controller;

namespace ConsoleView
{
    public class Program
    { 
        public static void Main()
        {
            Data.Initialize();
            Data.NextRace();

            Console.WriteLine(Data.CurrentRace.Track.Name);

            for (;;)
            {
                Thread.Sleep(100);
            }
        }
    }
}
