using System.Collections.Generic;
using System.Threading.Tasks;

namespace Overseer
{
    class Interfaces
    {
        public List<Interface> InterfaceList;

        public int InterfaceCount
        {
            get
            {
                return InterfaceList.Count;
            }
        }
        public Interfaces() : this(new List<Interface>()) { }

        public Interfaces(List<Interface> interfaceList)
        {
            InterfaceList = interfaceList;
        }

        public Interface GetInterfaceByName(string Name){
            foreach(var intface in InterfaceList){
                if(intface.Name == Name){
                    return intface;
                }
            }
            return null;
        }

        public List<string> GetInterfaceNames(){
            List<string> InterfaceNames = new List<string>();
            foreach(var intface in InterfaceList){
                InterfaceNames.Add(intface.Name);
            }
            return InterfaceNames;
        }
    
        public static Interfaces GetFromStringResult(string Result)
        {
            List<Interface> interfaceList = new List<Interface>();
            var splices = Result.Split('\n');
            Interface surrogateInterface = null;
            foreach(var splice in splices){
                var pieces = splice.Split(':');
                var splice0 = pieces[0];
                if (splice0 == "")
                {
                    continue;
                }
                else if(splice0.Contains("Name")){
                    surrogateInterface = new Interface
                    {
                        Name = pieces[pieces.Length - 1].Trim()
                    };
                }
                else if(splice0.Contains("Description")){
                    surrogateInterface.Description = pieces[pieces.Length - 1].Trim();
                }
                else if(splice0.Contains("State")){
                    if(pieces[pieces.Length - 1].Trim() == "connected"){
                        surrogateInterface.Connected = true;
                    }
                    else{
                        surrogateInterface.Connected = false;
                        interfaceList.Add(surrogateInterface);
                    }
                }
                else if(splice0.Contains("SSID") && !splice0.Contains("BSSID")){
                    surrogateInterface.SSIDName = pieces[pieces.Length - 1].Trim();
                    interfaceList.Add(surrogateInterface);
                }
            }
            return new Interfaces(interfaceList);
        }
        public static async Task<Interfaces> GetFromStringResultAsync(string Result){
            var interfaces = await Task.Run(() => (GetFromStringResult(Result)));
            return interfaces;
        }
    }
}