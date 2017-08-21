using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Metro_Navigation
{
    [Serializable]
    public class Station
    {
        public int StationID { get; set; } 
        public string Name { get; set; } 
        public Point Position { get; set; } 
        public bool IsTransferStation { get; set; } 
        public string TransferStationName { get; set; }
        
        
        public Station() { }

        public Station(int id, string name, Point position)
        {
            StationID = id;
            Name = name;
            Position = position;

        }
        public Station(int id, string name, Point position, string transferStation)
        {
            StationID = id;
            Name = name;
            Position = position;
            TransferStationName = transferStation;
        }

        public override string ToString()
        {
            return $"{Name} X:{Position.X}/Y:{Position.Y}";
        }
    }
}
