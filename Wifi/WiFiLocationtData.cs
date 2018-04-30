using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Wifi
{
    public class WiFiLocationtData
    {
       
        public Geoposition Position { get; set; }
        public byte SignalBars { get; set; }
        public double RssiInDecibelMilliwatts { get; set; }
    }
}
