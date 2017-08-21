using Metro_Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Metro_Navigation
{

    public class Editor
    {
        public List<SubwayLine> Lines = new List<SubwayLine>();
        public int NumOfLines { get; set; }
        public int SelectedLine { get; set; }
        public Station SelectedStation;

        public Editor(int LinesQuantity)
        {
            NumOfLines = LinesQuantity;
            for (int i = 0; i < NumOfLines; i++)
            {
                Lines.Add(new SubwayLine()
                {
                    SubwayLineID = i
                });
            }
            SelectedLine = 0;
        }

        public void ChangeLineColor(Color lineColor)
        {
            Lines[SelectedLine].LineColor = lineColor;
        }

        public bool AddStation(int id,string name,double x,double y,bool isTrans,string transStation = "")
        {

            if (Lines[SelectedLine].IDNotExist(id))
            {
                Lines[SelectedLine].Stations.Add(new Station
                {
                    Name = name,
                    StationID = id,
                    Position = new Point(x, y),
                    IsTransferStation = isTrans,
                    TransferStationName = isTrans ? transStation : null
                });
                if (isTrans)
                {
                    AddTransfer(name, transStation);
                }
                return true;
            }
            else throw new Exception("Такой ID уже существует!");
        }

        private void AddTransfer(string name, string transStation)
        {
            foreach (var item in Lines)
            {
                
                if(item.Stations.Exists(x => x.Name == transStation))
                {
                    int i = item.Stations.FindIndex(x => x.Name == transStation);
                    item.Stations[i].IsTransferStation = true;
                    item.Stations[i].TransferStationName = name;
                }
            }
        }

        public bool EditStation(int id, string name, double x, double y, bool isTrans, string transStation = "")
        {

            if ((id == SelectedStation.StationID) ||  Lines[SelectedLine].IDNotExist(id))
            {
                SelectedStation.Name = name;
                SelectedStation.StationID = id;
                SelectedStation.Position = new Point(x, y);
                SelectedStation.IsTransferStation = isTrans;
                SelectedStation.TransferStationName = isTrans ? transStation : null;
                if (isTrans) AddTransfer(name, transStation);
                return true;
            }
            else throw new Exception("Такой ID уже существует!");
        }

        public void DelStation()
        {
            Lines[SelectedLine].Stations.Remove(SelectedStation);
        }

        public void AddLine()
        {
            Lines.Add(new SubwayLine()
            {
                SubwayLineID = NumOfLines + 1
            });
            NumOfLines++;
            SelectedLine = 0;

        }

        public void DelLine()
        {
            if (NumOfLines > 1)
            {
                Lines.RemoveAt(NumOfLines - 1);
                NumOfLines++;
            }
            SelectedLine = 0;

        }


        public void DrowLines(Canvas field)
        {
            field.Children.Clear();
            foreach (var item in Lines)
            {
                foreach (var st in item.Stations)
                {
                    Ellipse el = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = new SolidColorBrush(item.LineColor)
                    };
                    Canvas.SetTop(el, st.Position.X);
                    Canvas.SetLeft(el, st.Position.Y);
                    Label name = new Label
                    {
                        Content = st.Name,
                        FontSize = 10
                    };
                    Canvas.SetTop(name, st.Position.X - 8);
                    Canvas.SetLeft(name, st.Position.Y + 6);
                    field.Children.Add(el);
                    field.Children.Add(name);
                }
            }
            
            
        }

        public void FillStationEdit(ComboBox comboEdit)
        {
            
            comboEdit.Items.Clear();
                foreach(var st in Lines[SelectedLine].Stations)
                {
                
                comboEdit.Items.Add(st.Name);
                }
            comboEdit.SelectedIndex = 0;
        }

        public void FillTransfers(ComboBox comboTrans)
        {

            comboTrans.Items.Clear();
            comboTrans.Items.Add("без пересадок");

            for (int i = 0; i < Lines.Count; i++)
            {
                if (i != SelectedLine)
                {
                    foreach (var item in Lines[i].Stations)
                    {
                        comboTrans.Items.Add(item.Name);
                       
                    }
                }
            }
            comboTrans.SelectedIndex = 0; 
        }
    

        public void OpenFile(string fileName)
        {
            Lines.Clear();
            Lines = Serializer.DeserializeAll<List<SubwayLine>>(fileName);
            NumOfLines = Lines.Count;
            SelectedLine = 0;
        }

        public Color GetCurentColor()
        {
            return Lines[SelectedLine].LineColor;
        }

        public void SelectStation(int i)
        {
            
            SelectedStation = Lines[SelectedLine].Stations[i];
        }



    }
}
