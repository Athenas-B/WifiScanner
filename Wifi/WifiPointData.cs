using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi
{
    public class WiFiLocationtData
    {
        public DateTimeOffset TimeStamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public byte SignalBars { get; set; }
        public double RssiInDecibelMilliwatts { get; set; }
    }
}
