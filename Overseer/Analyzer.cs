using System.Diagnostics;
using System.Threading.Tasks;

namespace Overseer
{
    static class Analyzer
    {
        public static async Task<string> getSSIDsAsync(string InterfaceName)
        {
            var res = await Task.Run(() => (getSSIDs(InterfaceName)));
            return res;
        }

        public static string getSSIDs(string InterfaceName)
        {
            Process P = new Process();
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.CreateNoWindow = true;
            PSI.FileName = "netsh.exe";
            PSI.Arguments = "wlan show networks mode=bssid interface=" + "\"" + InterfaceName  +"\"";
            PSI.RedirectStandardInput = true;
            PSI.RedirectStandardOutput = true;
            PSI.UseShellExecute = false;
            P.StartInfo = PSI;
            P.Start();
            var result = P.StandardOutput.ReadToEnd();
            P.WaitForExit();
            return result;
        }

        public static async Task<string> getInterfacesAsync()
        {
            var res = await Task.Run(() => (getInterfaces()));
            return res;
        }

        public static string getInterfaces()
        {
            Process P = new Process();
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.CreateNoWindow = true;
            PSI.FileName = "netsh.exe";
            PSI.Arguments = "wlan show interfaces";
            PSI.RedirectStandardInput = true;
            PSI.RedirectStandardOutput = true;
            PSI.UseShellExecute = false;
            P.StartInfo = PSI;
            P.Start();
            var result = P.StandardOutput.ReadToEnd();
            P.WaitForExit();
            return result;
        }
    }
}