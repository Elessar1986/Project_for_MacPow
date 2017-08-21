using Metro_Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Metro_Navigation
{
    class Navigation
    {
        public List<SubwayLine> Lines = new List<SubwayLine>();
        public int NumOfLines { get; set; }
        public List<Station> PathStations = new List<Station>();

        
        

        public Navigation()
        {

        }

        public void OpenFile(string fileName)
        {
            Lines.Clear();
            Lines = Serializer.DeserializeAll<List<SubwayLine>>(fileName);
            NumOfLines = Lines.Count;
            
        }

       

        private int NumOfLineByStation(string name)
        {
            return Lines.FindIndex(x => x.Stations.Exists(n => n.Name == name));
        }

        public void DrowMetro(Canvas field)
        {
            DrowLines(field);
            DrowStations(field);
        }

        private void DrowLines(Canvas field)
        {
            field.Children.Clear();
            foreach (var item in Lines)
            {
                foreach (var st in item.Stations.Where(x => x.IsTransferStation))
                {
                    int transLine = NumOfLineByStation(st.TransferStationName);
                    int transStation = Lines[transLine].Stations.FindIndex(x => x.Name == st.TransferStationName);
                    Line l = new Line
                    {
                        Y1 = st.Position.X + 5,
                        X1 = st.Position.Y + 5,
                        Y2 = Lines[transLine].Stations[transStation].Position.X + 5,
                        X2 = Lines[transLine].Stations[transStation].Position.Y + 5,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };
                    field.Children.Add(l);
                }

            }
            foreach (var item in Lines)
            {
                for(int i=0;i<item.Stations.Count;i++)
                {
                    
                    if (i != item.Stations.Count - 1)
                    {
                        Line l = new Line
                        {
                            Y1 = item.Stations[i].Position.X + 5,
                            X1 = item.Stations[i].Position.Y + 5,
                            Y2 = item.Stations[i + 1].Position.X + 5,
                            X2 = item.Stations[i + 1].Position.Y + 5,
                            Stroke = new SolidColorBrush(item.LineColor),
                            StrokeThickness = 1.5
                        };
                        field.Children.Add(l);
                    }
                    
                   
                }
            }
        }

        private void DrowStations(Canvas field)
        {
            foreach (var item in Lines)
            {
                for (int i = 0; i < item.Stations.Count; i++)
                {

                    Ellipse el = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = new SolidColorBrush(item.LineColor)
                    };
                    Canvas.SetTop(el, item.Stations[i].Position.X);
                    Canvas.SetLeft(el, item.Stations[i].Position.Y);
                    Label name = new Label
                    {
                        Content = item.Stations[i].Name,
                        FontSize = 10
                    };
                    Canvas.SetTop(name, item.Stations[i].Position.X - 8);
                    Canvas.SetLeft(name, item.Stations[i].Position.Y + 6);

                    field.Children.Add(el);
                    field.Children.Add(name);
                }
            }
        }

        public void GetPath(string from, string to)
        {

            int fromLine, toLine;
            int fromSt, toSt;
            PathStations.Clear();
            fromLine = NumOfLineByStation(from);
            toLine = NumOfLineByStation(to);
            fromSt = Lines[fromLine].Stations.FindIndex(x => x.Name == from);
            toSt = Lines[toLine].Stations.FindIndex(x => x.Name == to);
            int fromStId = Lines[fromLine].Stations[fromSt].StationID;
            int toStId = Lines[toLine].Stations[toSt].StationID;
            if (fromLine == toLine)
            {
                
                if (fromStId < toStId)
                {
                    for (int i = fromStId; i <= toStId; i++)
                    {
                        PathStations.Add(Lines[fromLine].Stations.Find(x => x.StationID == i));
                    }
                }
                else
                {
                    for (int i = fromStId; i >= toStId; i--)
                    {
                        PathStations.Add(Lines[fromLine].Stations.Find(x => x.StationID == i));
                    }
                }
            }
            else
            {
                int transferIdA = 0;
                int transferIdB = 0;
                int pathLenght = Lines[fromLine].Stations.Count + Lines[toLine].Stations.Count;
                
                foreach (var item in Lines[fromLine].Stations.Where(x => x.IsTransferStation))
                {
                    
                    if(toLine == NumOfLineByStation(item.TransferStationName))
                    {
                        Station transStation = Lines[toLine].Stations.Find(x => x.Name == item.TransferStationName);
                        int pl = Math.Abs(fromStId - item.StationID) + Math.Abs(toStId - transStation.StationID);
                        if (pathLenght >= pl)
                        {
                            pathLenght = pl;
                            transferIdA = item.StationID;
                            transferIdB = transStation.StationID;
                        }
                    }
                }

                if(fromStId < transferIdA)
                {
                    for (int i = fromStId; i <= transferIdA; i++)
                    {
                        PathStations.Add(Lines[fromLine].Stations.Find(x => x.StationID == i));
                    }
                }
                else
                {
                    for (int i = fromStId; i >= transferIdA; i--)
                    {
                        PathStations.Add(Lines[fromLine].Stations.Find(x => x.StationID == i));
                    }
                }
                if (toStId > transferIdB)
                {
                    for (int i = transferIdB; i <= toStId; i++)
                    {
                        PathStations.Add(Lines[toLine].Stations.Find(x => x.StationID == i));
                    }
                }
                else
                {
                    for (int i = transferIdB; i >= toStId; i--)
                    {
                        PathStations.Add(Lines[toLine].Stations.Find(x => x.StationID == i));
                    }
                }


            }
            
        }

        public void DrowPath(Canvas field)
        {
            DrowLines(field);
            
            for(int i=0;i<PathStations.Count-1;i++)
            {
                Line l = new Line
                {
                    Y1 = PathStations[i].Position.X + 5,
                    X1 = PathStations[i].Position.Y + 5,
                    Y2 = PathStations[i + 1].Position.X + 5,
                    X2 = PathStations[i + 1 ].Position.Y + 5,
                    Stroke = Brushes.LightGreen,
                    StrokeThickness = 5
                };
                field.Children.Add(l);
            }
            DrowStations(field);
        }

        public void FillStations(ComboBox comboStations, string exeptionStation = null)
        {
            comboStations.ItemsSource = null;
            List<string> stationsList = new List<string>();
            foreach (var subline in Lines)
            {
                foreach (var st in subline.Stations)
                {
                    if(st.Name != exeptionStation)
                    {
                        stationsList.Add(st.Name);
                        
                    }
                }
            }
            stationsList.Sort();
            comboStations.ItemsSource = stationsList;
            comboStations.SelectedIndex = 0;
        }
    }
}
