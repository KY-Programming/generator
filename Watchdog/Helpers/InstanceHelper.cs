using System.Diagnostics;

namespace KY.Generator.Watchdog.Helpers
{
    public static class InstanceHelper
    {
        public static bool IsRunning()
        {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
        }
    }
}