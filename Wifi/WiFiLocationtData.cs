using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Wifi
{
    [DataContract]
    public class WiFiLocationtData
    {
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public byte SignalBars { get; set; }
        [DataMember]
        public double RssiInDecibelMilliwatts { get; set; }
    }
}
