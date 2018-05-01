using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Networking.Connectivity;

namespace Wifi
{
    [DataContract]
    [KnownType(typeof(WiFiLocationtData))]
    [KnownType(typeof(HashSet<WiFiLocationtData>))]
    public class WiFiSignalInfo : IEquatable<WiFiSignalInfo>
    {
        [DataMember]
        public string MacAddress { get; set; }
        [DataMember]
        public string Ssid { get; set; }
        [DataMember]
        public string NetworkKind { get; set; }
        [DataMember]
        public string PhysicalKind { get; set; }
        [DataMember]
        public byte SignalBars { get; set; }
        [DataMember]
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        [DataMember]
        public string NetworkAuthenticationType { get; set; }
        [DataMember]
        public string NetworkEncryptionType { get; set; }
        [DataMember]
        public double RssiInDecibelMilliwatts { get; set; }
        [DataMember]
        public HashSet<WiFiLocationtData> LocationData { get; set; }

        public WiFiSignalInfo()
        {
            LocationData = new HashSet<WiFiLocationtData>();
           
        }

        public override string ToString()
        {

            return Ssid +" : "+ MacAddress + " : " + NetworkKind + " : " + PhysicalKind + " : " + SignalBars + " : "  + ChannelCenterFrequencyInKilohertz  
                + " : " + RssiInDecibelMilliwatts + " : " + NetworkEncryptionType + " : " + NetworkAuthenticationType ;
        }

        public string GetTextDetail()
        {
            return Ssid + " : " + MacAddress + " : " + NetworkKind + " : " + PhysicalKind + " : " + SignalBars + " : " + ChannelCenterFrequencyInKilohertz
                + " : " + RssiInDecibelMilliwatts + " : " + NetworkEncryptionType + " : " + NetworkAuthenticationType;
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
            //hashCode = hashCode * -1521134295 + EqualityComparer<NetworkSecuritySettings>.Default.GetHashCode(NetworkSecuritySettings);
            return hashCode;
        }
    }
   
}
