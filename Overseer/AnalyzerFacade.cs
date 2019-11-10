namespace Overseer
{
    static class AnalyzerFacade
    {
        public static Interfaces GetInterfaces()
        {
            var UnformattedInterfaces = Analyzer.GetInterfaces();
            var FormattedInterfaces = Interfaces.GetFromStringResult(UnformattedInterfaces);
            return FormattedInterfaces;
        }
        public static Stations GetStations(string interfaceName)
        {
            var SSIDGetterTask = Analyzer.GetSSIDsAsync(interfaceName);
            SSIDGetterTask.Wait();
            var StationFormatterTask = Stations.GetFromStringResultAsync(SSIDGetterTask.Result);
            return StationFormatterTask.Result;
        }
    }
}
