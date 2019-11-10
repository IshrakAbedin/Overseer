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
        Bitmap blankGraph;
        Bitmap drawnGraph;
        Bitmap highlightedGraph;
        ImageSource srcBlankGraph;
        ImageSource srcDrawnGraph;
        ImageSource srcHighlightedGraph;
        System.Drawing.Color graphBaseColor;
        System.Drawing.Color graphHighlightColor;
        public MainWindow()
        {
            graphBaseColor = System.Drawing.Color.FromArgb(255, 0, 255, 195);
            graphHighlightColor = System.Drawing.Color.FromArgb(255, 0, 182, 255);
            blankGraph = GraphCreator.GetBlankGraph(800, 270);
            GraphCreator.UpdateImageSourceFromBitmap(blankGraph, ref srcBlankGraph);
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
            LstBxAPs.ItemsSource = null;
            ImgGraph.Source = srcBlankGraph;
            //ComBxInterfaces.ItemsSource = null;
        }

        private async void UpdateStations()
        {
            int selectedIndex = ComBxInterfaces.SelectedIndex;
            if (selectedIndex != -1)
            {
                string intfaceName = interfaceNames[selectedIndex];
                if (intfaceName != null || intfaceName != "")
                {
                    stations = await Task.Run(() => (AnalyzerFacade.GetStations(intfaceName)));
                }
                else stations = null;

                if (stations != null)
                {
                    apNames = await Task.Run(() => stations.GetSSIDNames());
                    TbInterfaceDesc.Text = interfaces.InterfaceList[selectedIndex].Description;
                    if (!stations.PoweredDown)
                    {
                        TbMsg.Text = stations.Message;
                        drawnGraph = null;
                        srcDrawnGraph = null;
                        drawnGraph = await Task.Run(() => GraphCreator.GetDrawnGraph(blankGraph, stations.GetSSIDChannelSignalPairList(), graphBaseColor));
                        GraphCreator.UpdateImageSourceFromBitmap(drawnGraph, ref srcDrawnGraph);
                        ImgGraph.Source = srcDrawnGraph;
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

        private async void UpdateAPView()
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
                var chnsigpair = selectedStation.GetChannelSignalPair();
                if(chnsigpair != null)
                {
                    highlightedGraph = null;
                    srcHighlightedGraph = null;
                    highlightedGraph = await Task.Run(() => GraphCreator.GetDrawnGraph(drawnGraph, chnsigpair, graphHighlightColor));
                    GraphCreator.UpdateImageSourceFromBitmap(highlightedGraph, ref srcHighlightedGraph);
                    ImgGraph.Source = srcHighlightedGraph;
                }
            }
        }

        private void MainDow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDow.DragMove();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
