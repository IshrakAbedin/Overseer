using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overseer
{
    static class AnalyzerFacade
    {
        //public static Interfaces getInterfaces()
        //{
        //    var InterfaceGetterTask = Analyzer.getInterfacesAsync();
        //    InterfaceGetterTask.Wait();
        //    var InterfaceFormatterTask = Interfaces.getFromStringResultAsync(InterfaceGetterTask.Result);
        //    return InterfaceFormatterTask.Result;
        //}
        public static Interfaces GetInterfaces()
        {
            var UnformattedInterfaces = Analyzer.getInterfaces();
            var FormattedInterfaces = Interfaces.GetFromStringResult(UnformattedInterfaces);
            return FormattedInterfaces;
        }
        public static Stations GetStations(string interfaceName)
        {
            var SSIDGetterTask = Analyzer.getSSIDsAsync(interfaceName);
            SSIDGetterTask.Wait();
            var StationFormatterTask = Stations.GetFromStringResultAsync(SSIDGetterTask.Result);
            return StationFormatterTask.Result;
        }
    }
}
