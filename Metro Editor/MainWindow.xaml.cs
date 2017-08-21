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
using MahApps.Metro.Controls;
//using Xceed.Wpf.Toolkit;
using Metro_Navigation;
using Microsoft.Win32;

namespace Metro_Navigation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Editor metro = new Editor(1);
        
        

        public MainWindow()
        {
            InitializeComponent();
            SubwayLinesQuantity.Value = 1;
            SubwayLineNum.Items.Clear();
            for (int i = 0; i < metro.NumOfLines; i++)
            {
                SubwayLineNum.Items.Add(i);
            }
            SubwayLineNum.SelectedIndex = 0;

        }

        private void SubwayLinesQuantity_ValueDecremented(object sender, NumericUpDownChangedRoutedEventArgs args)
        {
            metro.AddLine();

            
            SubwayLineNum.Items.Clear();
            for (int i = 0; i < metro.NumOfLines; i++)
            {
                SubwayLineNum.Items.Add(i);
            }
            SubwayLineNum.SelectedIndex = 0;
        }

        private void SubwayLinesQuantity_ValueIncremented(object sender, NumericUpDownChangedRoutedEventArgs args)
        {
            metro.DelLine();
            SubwayLineNum.Items.Clear();
            for (int i = 0; i < metro.NumOfLines; i++)
            {
                SubwayLineNum.Items.Add(i);
            }
            SubwayLineNum.SelectedIndex = 0;
        }

        private void SubwayLinesQuantity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
           

        }

        private void SubwayLineNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubwayLineNum.SelectedIndex != -1)
            {
                metro.SelectedLine = SubwayLineNum.SelectedIndex;
                ColorPicker1.SelectedColor = metro.GetCurentColor();
                metro.FillStationEdit(EditStationName);
                metro.FillTransfers(TransferAdd);
                metro.FillTransfers(TransferEdit);
                EditStationName_SelectionChanged(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TransferAdd.SelectedIndex <= 0) metro.AddStation((Int32)ID.Value, StationName.Text, (Int32)X.Value, (Int32)Y.Value, false);
                else metro.AddStation((Int32)ID.Value, StationName.Text, (Int32)X.Value, (Int32)Y.Value, true, TransferAdd.SelectedValue.ToString());
                metro.DrowLines(myCanvas);
                metro.FillTransfers(TransferAdd);
                metro.FillTransfers(TransferEdit);
                metro.FillStationEdit(EditStationName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TransferEdit.SelectedIndex <= 0) metro.EditStation((Int32)editID.Value, EditStationName.Text, (Int32)editX.Value, (Int32)editY.Value, false);
                else metro.EditStation((Int32)editID.Value, EditStationName.Text, (Int32)editX.Value, (Int32)editY.Value, true, TransferEdit.SelectedValue.ToString());
                metro.DrowLines(myCanvas);
                metro.FillStationEdit(EditStationName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            metro.DelStation();
            metro.FillStationEdit(EditStationName);
            if (EditStationName.SelectedIndex != -1)
            {
                metro.SelectStation(EditStationName.SelectedIndex);
                editID.Value = metro.SelectedStation.StationID;
                editX.Value = metro.SelectedStation.Position.X;
                editY.Value = metro.SelectedStation.Position.Y;
                if (metro.SelectedStation.IsTransferStation)
                {
                    TransferEdit.SelectedValue = metro.SelectedStation.TransferStationName;
                }
                else TransferEdit.SelectedIndex = 0;
            }

        }

        private void ColorPicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            metro.ChangeLineColor(ColorPicker1.SelectedColor.Value);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void EditStationName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditStationName.SelectedIndex != -1)
            {
                metro.SelectStation(EditStationName.SelectedIndex);
                editID.Value = metro.SelectedStation.StationID;
                editX.Value = metro.SelectedStation.Position.X;
                editY.Value = metro.SelectedStation.Position.Y;
                if (metro.SelectedStation.IsTransferStation)
                {
                    TransferEdit.SelectedValue = metro.SelectedStation.TransferStationName;
                }
                else TransferEdit.SelectedIndex = 0;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "XML File|*.xml"
            };
            if(sfd.ShowDialog() == true) 
            Serializer.SerializeAll(metro.Lines,sfd.FileName);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML File|*.xml"
            };
            if(ofd.ShowDialog() == true)
            {
                metro.OpenFile(ofd.FileName);
                SubwayLinesQuantity.Value = metro.NumOfLines;
                SubwayLineNum.Items.Clear();
                for (int i = 0; i < metro.NumOfLines; i++)
                {
                    SubwayLineNum.Items.Add(i);
                }
                SubwayLineNum.SelectedIndex = 0;
                ColorPicker1.SelectedColor = metro.GetCurentColor();
                metro.DrowLines(myCanvas);
                metro.FillTransfers(TransferAdd);
                metro.FillTransfers(TransferEdit);
                metro.FillStationEdit(EditStationName);
            }
                
        }

        private void TransferEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        
    }
}
