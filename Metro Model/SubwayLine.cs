using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Metro_Navigation;

namespace Metro_Navigation
{
    [Serializable]
    public class SubwayLine
    {
        public int SubwayLineID { get; set; }
        public string Name { set; get; }
        public List<Station> Stations { get; set; }
        public Color LineColor { get; set; }

        public SubwayLine()
        {
            Stations = new List<Station>();
            LineColor = Colors.Black;
        }

        public SubwayLine(string name, List<Station> stations)
        {
            Name = name;
            Stations = stations;
        }

        public bool IDNotExist(int id)
        {
            foreach(var i in Stations)
            {
                if (i.StationID == id) return false;
            }
            return true;
        }
    }
}
