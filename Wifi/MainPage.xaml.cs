using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;

// Dokumentaci k šabloně položky Prázdná stránka najdete na adrese https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x405

namespace Wifi
{
    /// <summary>
    /// Prázdná stránka, která se dá použít samostatně nebo v rámci objektu Frame
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WifiScanner WifiScanner;
        private WiFiSignalInfo selectedWifi;
        public MainPage()
        {
            this.InitializeComponent();
            WifiScanner = new WifiScanner();
            MapControl.MapServiceToken = "Ai-JrzIrH33ZLoj7rtSUdwLYliMcYfetOTp_cGmu85gWntcUSD6SOB1OmiSw3eB6";

            WifiList.Items.Clear();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Tick += NetworkScan;
            timer.Start();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           

           // MapControl.MapServiceToken = "Ai-JrzIrH33ZLoj7rtSUdwLYliMcYfetOTp_cGmu85gWntcUSD6SOB1OmiSw3eB6";
        }

        private async void NetworkScan(object sender, object e)
        {
            await WifiScanner.ScanForNetworks();
            WifiList.Items.Clear();
            foreach (var item in WifiScanner.Wifi)
            {
                WifiList.Items.Add(item);

                if (item.Equals(selectedWifi)) { //if there is selected network refresh its info
                    selectedWifi = item;
                    WifiDetail.Text = selectedWifi.GetTextDetail();
                    WifiList.SelectedItem = selectedWifi;
                }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void WifiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WifiList.SelectedItem != null)
            {
                selectedWifi = (WiFiSignalInfo)WifiList.SelectedItem;
            }
            WifiDetail.Text = selectedWifi.GetTextDetail();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            selectedWifi.LocationData.Clear();
        }
    }
}
