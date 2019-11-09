using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Overseer
{
    static class Analyzer
    {
        public static async Task<string> GetSSIDsAsync(string InterfaceName)
        {
            var res = await Task.Run(() => (GetSSIDs(InterfaceName)));
            return res;
        }

        public static string GetSSIDs(string InterfaceName)
        {
            try
            {
                Process P = new Process();
                ProcessStartInfo PSI = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "netsh.exe",
                    Arguments = "wlan show networks mode=bssid interface=" + "\"" + InterfaceName + "\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                P.StartInfo = PSI;
                P.Start();
                var result = P.StandardOutput.ReadToEnd();
                P.WaitForExit();
                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find netsh.exe, Exiting", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            return null;
        }

        public static async Task<string> GetInterfacesAsync()
        {
            var res = await Task.Run(() => (GetInterfaces()));
            return res;
        }

        public static string GetInterfaces()
        {
            try
            {
                Process P = new Process();
                ProcessStartInfo PSI = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "netsh.exe",
                    Arguments = "wlan show interfaces",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                P.StartInfo = PSI;
                P.Start();
                var result = P.StandardOutput.ReadToEnd();
                P.WaitForExit();
                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find netsh.exe, Exiting", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            return null;
        }
    }
}