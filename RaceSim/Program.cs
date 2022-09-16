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

            Console.WriteLine(Data.CurrentRace.Track.Name);

            for (;;)
            {
                Thread.Sleep(100);
                Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace);
            }
        }
    }
}
