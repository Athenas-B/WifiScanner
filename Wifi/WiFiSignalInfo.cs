using System;
using System.Collections.Generic;
using Windows.Networking.Connectivity;

namespace Wifi
{

        public class WiFiSignalInfo : IEquatable<WiFiSignalInfo>
    {
        public string MacAddress { get; set; }
        public string Ssid { get; set; }
        public string NetworkKind { get; set; }
        public string PhysicalKind { get; set; }
        public byte SignalBars { get; set; }
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        public NetworkSecuritySettings NetworkSecuritySettings { get; set; }
        public double RssiInDecibelMilliwatts { get; set; }
        public HashSet<WiFiLocationtData> LocationData { get; set; }

        public override string ToString()
        {
            return Ssid +" : "+ MacAddress + " : " + NetworkKind + " : " + PhysicalKind + " : " + SignalBars + " : "  + ChannelCenterFrequencyInKilohertz  
                + " : " + RssiInDecibelMilliwatts + " : " + NetworkSecuritySettings.NetworkEncryptionType + " : " + NetworkSecuritySettings.NetworkAuthenticationType ;
        }

        public string GetTextDetail()
        {
            return Ssid + " : " + MacAddress + " : " + NetworkKind + " : " + PhysicalKind + " : " + SignalBars + " : " + ChannelCenterFrequencyInKilohertz
                + " : " + RssiInDecibelMilliwatts + " : " + NetworkSecuritySettings.NetworkEncryptionType + " : " + NetworkSecuritySettings.NetworkAuthenticationType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WiFiSignalInfo);
        }

        public bool Equals(WiFiSignalInfo other)
        {
            return other != null &&
                   MacAddress == other.MacAddress &&
                   Ssid == other.Ssid &&
                   NetworkKind == other.NetworkKind &&
                   PhysicalKind == other.PhysicalKind;//&&
                   //EqualityComparer<NetworkSecuritySettings>.Default.Equals(NetworkSecuritySettings, other.NetworkSecuritySettings);
                   //NetworkSecuritySettings.Equals(NetworkSecuritySettings, other.NetworkSecuritySettings);
        }

        public override int GetHashCode()
        {
            var hashCode = -875630175;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MacAddress);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Ssid);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NetworkKind);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PhysicalKind);
            hashCode = hashCode * -1521134295 + EqualityComparer<NetworkSecuritySettings>.Default.GetHashCode(NetworkSecuritySettings);
            return hashCode;
        }
    }
   
}
