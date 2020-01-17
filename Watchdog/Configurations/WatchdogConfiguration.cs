using System;
using KY.Generator.Configurations;

namespace KY.Generator.Watchdog.Configurations
{
    public class WatchdogConfiguration : ConfigurationBase
    {
        public bool Async { get; set; }
        public string LaunchSettings { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan Sleep { get; set; } = TimeSpan.FromMilliseconds(100);
        public string Command { get; set; }
        public string Url { get; set; }
        public int Tries { get; set; }
    }
}