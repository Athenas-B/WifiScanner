using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Devices.WiFi;
using Windows.UI.Xaml;

namespace Wifi
{
    partial class WifiScanner
    {

        public WiFiAdapter WiFiAdapter { get; private set; }
      
        public List<WiFiSignalInfo> Wifi { get; private set; }


        public WifiScanner()
        {
            Wifi = new List<WiFiSignalInfo>();
            InitializeFirstAdapter();
        }

        public async Task InitializeFirstAdapter()
        {
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                var wifiAdapterResults =
                  await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (wifiAdapterResults.Count >= 1)
                {
                    this.WiFiAdapter =
                      await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
                }
                else
                {
                    throw new Exception("WiFi Adapter not found.");
                }
            }
        }

        public async Task ScanWifiRepeteadly(int seconds) {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, seconds);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        public async Task StopScanning() { }

        private async void Timer_Tick(object sender, object e)
        {
            await ScanForNetworks();
        }

        public async Task ScanForNetworks()
        {
            Wifi.Clear();
            if (this.WiFiAdapter != null)
            {
                await this.WiFiAdapter.ScanAsync();
                var report = WiFiAdapter.NetworkReport;
                Geolocator geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();

                foreach (var availableNetwork in report.AvailableNetworks)
                {

                    WiFiSignalInfo wifiSignal = new WiFiSignalInfo()
                    {
                        MacAddress = availableNetwork.Bssid,
                        Ssid = availableNetwork.Ssid,
                        SignalBars = availableNetwork.SignalBars,
                        ChannelCenterFrequencyInKilohertz =
                        availableNetwork.ChannelCenterFrequencyInKilohertz,
                        NetworkKind = availableNetwork.NetworkKind.ToString(),
                        PhysicalKind = availableNetwork.PhyKind.ToString(),
                        NetworkSecuritySettings = availableNetwork.SecuritySettings,
                        RssiInDecibelMilliwatts = availableNetwork.NetworkRssiInDecibelMilliwatts
                        
                    };
                    Wifi.Add(wifiSignal);
                    
                    
                }
                

            }
        }
    }
}
