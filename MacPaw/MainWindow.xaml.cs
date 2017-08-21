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
using Metro_Navigation;
using Microsoft.Win32;

namespace Metro_Navigation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        Navigation metro = new Navigation();
        
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML File|*.xml"
            };
            if (ofd.ShowDialog() == true)
            {
                from_station.ItemsSource = null;
                to_station.ItemsSource = null;
                metro.OpenFile(ofd.FileName);
                metro.DrowMetro(myCanvas);
                metro.FillStations(from_station);
                metro.FillStations(to_station, from_station.SelectedValue.ToString());
            }
        }

        private void from_station_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(from_station.SelectedIndex != -1)
            metro.FillStations(to_station, from_station.SelectedValue.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            metro.GetPath(from_station.SelectedValue.ToString(), to_station.SelectedValue.ToString());
            metro.DrowPath(myCanvas);
        }
    }
}
