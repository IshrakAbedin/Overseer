using System.Collections.Generic;

namespace Overseer
{
    class SSID
    {
        public string Name { get; set; }
        public string NetType { get; set; }
        public string Authentication { get; set; }
        public string Encryption { get; set; }
        public List<BSSID> BSSIDList = new List<BSSID>();

        public SSID() : this("", "", "", "", new List<BSSID>()) { }

        public SSID(string name, string netType, string authentication, string encryption, List<BSSID> bSSIDList)
        {
            Name = name;
            NetType = netType;
            Authentication = authentication;
            Encryption = encryption;
            BSSIDList = bSSIDList;
        }

        public Pair GetChannelSignalPair()
        {
            foreach(var bssid in BSSIDList)
            {
                if(bssid.Channel > 0 && bssid.Channel <= 13)
                {
                    int sig = 0;
                    if(bssid.Signal >= 0 && bssid.Signal <= 100)
                    {
                        sig = bssid.Signal;
                    }
                    return new Pair(bssid.Channel, sig);
                }
            }
            return null;
        }
    }
}