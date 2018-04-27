using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi
{
    public class WiFiPointData
    {
        public DateTimeOffset TimeStamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public List<WiFiSignalInfo> WiFiSignals { get; set; }
        public WiFiPointData()
        {
            this.WiFiSignals = new List<WiFiSignalInfo>();
        }
    }
}
