using Windows.Networking.Connectivity;

namespace Wifi
{

        public class WiFiSignalInfo
        {
        public string MacAddress { get; set; }
        public string Ssid { get; set; }
        public string NetworkKind { get; set; }
        public string PhysicalKind { get; set; }
        public byte SignalBars { get; set; }
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        public NetworkSecuritySettings NetworkSecuritySettings { get; set; }
        public double RssiInDecibelMilliwatts { get; set; }

        public override string ToString()
        {
            return Ssid +" : "+ MacAddress + " : " + NetworkKind + " : " + PhysicalKind + " : " + SignalBars + " : "  + ChannelCenterFrequencyInKilohertz  
                + " : " + RssiInDecibelMilliwatts + " : " + NetworkSecuritySettings.NetworkEncryptionType + " : " + NetworkSecuritySettings.NetworkAuthenticationType ;
        } 
    }
   
}
