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
        public MainPage()
        {
            this.InitializeComponent();
            WifiScanner = new WifiScanner();
            MapControl.MapServiceToken = "Ai-JrzIrH33ZLoj7rtSUdwLYliMcYfetOTp_cGmu85gWntcUSD6SOB1OmiSw3eB6";

            WifiList.Items.Clear();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           

           // MapControl.MapServiceToken = "Ai-JrzIrH33ZLoj7rtSUdwLYliMcYfetOTp_cGmu85gWntcUSD6SOB1OmiSw3eB6";
        }

        private async void Timer_Tick(object sender, object e)
        {
            await WifiScanner.ScanForNetworks();
            WifiList.Items.Clear();
            foreach (var item in WifiScanner.Wifi)
            {
                WifiList.Items.Add(item);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WifiScanner.StopScanning();
        }

    }
}
