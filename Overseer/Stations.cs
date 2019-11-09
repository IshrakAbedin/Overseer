using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Overseer
{
    class Stations
    {
        public string InterfaceName { get; set; }
        public string Message { get; set; }
        public bool PoweredDown { get; set; }

        public int StationCount
        {
            get
            {
                return StationList.Count;
            }
        }
        public List<SSID> StationList;

        public Stations() : this("", "", true, new List<SSID>()) { }

        public Stations(string interfaceName, string message, bool poweredDown, List<SSID> stationList)
        {
            InterfaceName = interfaceName;
            Message = message;
            PoweredDown = poweredDown;
            StationList = stationList;
        }

        public SSID GetSSIDbyName(string Name){
            foreach(var station in StationList){
                if(station.Name == Name){
                    return station;
                }
            }
            return null;
        }

        public List<string> GetSSIDNames(){
            List<string> stationNames = new List<string>();
            foreach(var station in StationList){
                stationNames.Add(station.Name);
            }
            return stationNames;
        }

        public List<Pair> GetSSIDChannelSignalPairList()
        {
            List<Pair> chnsiglst = new List<Pair>();
            foreach(var station in StationList)
            {
                var chnsigpair = station.GetChannelSignalPair();
                if(chnsigpair != null)
                {
                    chnsiglst.Add(chnsigpair);
                }
            }
            return chnsiglst;
        }

        public static Stations GetFromStringResult(string Result)
        {
            string interfaceName = "";
            string msg = "";
            bool poweredDown = false;
            var splices = Result.Split('\n');
            List<SSID> stationList = new List<SSID>();
            SSID surrogateSSID = null;
            BSSID surrogateBSSID = null;
            foreach (var splice in splices)
            {
                var splice0 = splice.Split(':')[0];
                //System.Console.WriteLine(splice);
                //System.Console.WriteLine("-------------------");
                if (splice0 == "")
                {
                    continue;
                }
                else if (splice0.Contains("Interface name"))
                {
                    var pieces = splice.Split(':');
                    interfaceName = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("powered down"))
                {
                    msg = splice0.Trim();
                    poweredDown = true;
                    break;
                }
                else if (splice0.Contains("currently visible"))
                {
                    msg = splice0.Trim().Trim('.');
                    poweredDown = false;
                }
                else if (splice0.Contains("SSID") && !splice.Contains("BSSID"))
                {
                    if (surrogateSSID != null)
                    {
                        stationList.Add(surrogateSSID);
                    }
                    surrogateSSID = new SSID();
                    var pieces = splice.Split(':');
                    surrogateSSID.Name = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("Network type"))
                {
                    var pieces = splice.Split(':');
                    surrogateSSID.NetType = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("Authentication"))
                {
                    var pieces = splice.Split(':');
                    surrogateSSID.Authentication = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("Encryption"))
                {
                    var pieces = splice.Split(':');
                    surrogateSSID.Encryption = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("BSSID"))
                {
                    surrogateBSSID = new BSSID();
                    var pieces = splice.Split(':');
                    string tempMAC = "";
                    for (int i = 1; i < pieces.Length; i++)
                    {
                        tempMAC += pieces[i].Trim();
                        if (i != pieces.Length - 1)
                            tempMAC += ':';
                    }
                    surrogateBSSID.MAC = tempMAC;
                }
                else if (splice0.Contains("Signal"))
                {
                    var pieces = splice.Split(':');
                    try
                    {
                        surrogateBSSID.Signal = Convert.ToUInt16(pieces[pieces.Length - 1].Replace('%', ' ').Trim());
                    }
                    catch (Exception)
                    {
                        surrogateBSSID.Signal = 255;
                    }
                }
                else if (splice0.Contains("Radio type"))
                {
                    var pieces = splice.Split(':');
                    surrogateBSSID.RadioType = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("Channel"))
                {
                    var pieces = splice.Split(':');
                    surrogateBSSID.Channel = Convert.ToUInt16(pieces[pieces.Length - 1].Trim());
                }
                else if (splice0.Contains("Basic rates"))
                {
                    var pieces = splice.Split(':');
                    surrogateBSSID.BasicRates = pieces[pieces.Length - 1].Trim();
                }
                else if (splice0.Contains("Other rates"))
                {
                    var pieces = splice.Split(':');
                    surrogateBSSID.OtherRates = pieces[pieces.Length - 1].Trim();
                    surrogateSSID.BSSIDList.Add(surrogateBSSID);
                }
            }
            if (surrogateSSID != null)
            {
                stationList.Add(surrogateSSID);
            }

            return new Stations(interfaceName, msg, poweredDown, stationList);
        }

        public static async Task<Stations> GetFromStringResultAsync(string Result)
        {
            var stations = await Task.Run(() => (GetFromStringResult(Result)));
            return stations;
        }
    }
}