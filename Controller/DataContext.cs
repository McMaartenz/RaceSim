using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class DataContext : INotifyPropertyChanged
    {
        public string TrackName { get; set; }

        public DataContext()
        {
            TrackName = "UNTITLED";

            Data.RaceChanged += (sender, e) =>
            {
                TrackName = e.race.Track.Name;
                e.race.DriversChanged += (sender, e) => PropertyChanged?.Invoke(this, new(""));
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
