using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Overseer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> interfaceNames;
        List<string> apNames;
        Stations stations;
        Interfaces interfaces;
        GraphManager graphMan;
        List<GraphOutput> gout;
        List<GraphInput> gin;
        SolidColorBrush baseFillBrush;
        SolidColorBrush baseStrokeBrush;
        SolidColorBrush highlightedFillBrush;
        SolidColorBrush highlightedStrokeBrush;
        string previousSelection = "";

        public MainWindow()
        {
            baseFillBrush = new SolidColorBrush(Color.FromArgb(30, 0, 255, 195));
            baseStrokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 195));
            highlightedFillBrush = new SolidColorBrush(Color.FromArgb(30, 0, 180, 255));
            highlightedStrokeBrush = Brushes.BlueViolet;
            gin = new List<GraphInput>();
            graphMan = new GraphManager(44, 45, 800, 270, baseFillBrush, baseStrokeBrush, highlightedFillBrush, highlightedStrokeBrush, 10)
            {
                BaseLabelBrush = Brushes.White,
                HighlightedLabelBrush = Brushes.BlueViolet
            };
            InitializeComponent();
            ResetView();
            UpdateInterfaceComboBox();
        }

        private void ResetView()
        {
            TbAP.Text = "---";
            TbNetType.Text = "---";
            TbAuthentication.Text = "---";
            TbEncryption.Text = "---";
            TbBSSID.Text = "---";
            TbSignal.Text = "---";
            TbRadioType.Text = "---";
            TbChannel.Text = "---";
            TbBasicRates.Text = "---";
            TbOtherRates.Text = "---";
            TbInterfaceDesc.Text = "---";
            TbMsg.Text = "";
            previousSelection = "";
            LstBxAPs.ItemsSource = null;
            CanvasGraph.Children.Clear();
        }

        private async void UpdateStations()
        {
            int selectedIndex = ComBxInterfaces.SelectedIndex;
            if (selectedIndex != -1)
            {
                string intfaceName = interfaceNames[selectedIndex];
                if (intfaceName != null || intfaceName != "")
                {
                    if (stations != null) stations.StationList.Clear();
                    stations = null;
                    stations = await Task.Run(() => (AnalyzerFacade.GetStations(intfaceName)));
                }
                else stations = null;

                if (stations != null)
                {
                    if (apNames != null) apNames.Clear();
                    apNames = null;
                    apNames = await Task.Run(() => stations.GetSSIDNames());
                    TbInterfaceDesc.Text = interfaces.InterfaceList[selectedIndex].Description;
                    if (!stations.PoweredDown)
                    {
                        TbMsg.Text = stations.Message;
                        gin.Clear();
                        foreach(var station in stations.StationList)
                        {
                            gin.Add(new GraphInput(station.Name, station.GetChannelSignalPair()));
                        }
                        gout = graphMan.GetGraphOutput(gin);
                        foreach (var graphOut in gout)
                        {
                            CanvasGraph.Children.Add(graphOut.Lbel);
                            CanvasGraph.Children.Add(graphOut.PGon);
                        }
                    }
                    else TbMsg.Text = "Wireless network service is currently powered down";
                }
                else
                {
                    apNames = null;
                }
                LstBxAPs.ItemsSource = apNames;
            }
            else return;
        }

        private async void UpdateInterfaceComboBox()
        {
            interfaces = await Task.Run(() => (AnalyzerFacade.GetInterfaces()));
            interfaceNames = interfaces.GetInterfaceNames();
            ComBxInterfaces.ItemsSource = interfaceNames;
            if (interfaceNames.Count > 0)
            {
                ComBxInterfaces.SelectedIndex = 0;
            }
        }

        private void UpdateAPView()
        {
            if(stations != null && LstBxAPs.SelectedIndex != -1)
            {
                var selectedStation = stations.StationList[LstBxAPs.SelectedIndex];
                TbAP.Text = selectedStation.Name;
                TbNetType.Text = selectedStation.NetType;
                TbAuthentication.Text = selectedStation.Authentication;
                TbEncryption.Text = selectedStation.Encryption;
                foreach(var bssid in selectedStation.BSSIDList)
                {
                    if(bssid.Channel <= 13)
                    {
                        TbBSSID.Text = bssid.MAC;
                        TbSignal.Text = bssid.Signal.ToString() + "%";
                        TbRadioType.Text = bssid.RadioType;
                        TbChannel.Text = bssid.Channel.ToString();
                        TbBasicRates.Text = bssid.BasicRates;
                        TbOtherRates.Text = bssid.OtherRates;
                        break;
                    }
                }
                graphMan.HighlightGraph(selectedStation.Name, previousSelection);
                previousSelection = selectedStation.Name;
            }
        }

        private void MainDow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDow.DragMove();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ResetView();
            UpdateStations();
        }

        private void BtnRefInt_Click(object sender, RoutedEventArgs e)
        {
            UpdateInterfaceComboBox();
        }

        private void LstBxAPs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAPView();
        }
    }
}
