using System;

namespace KY.Generator.Settings
{
    public class GlobalSettings
    {
        public bool StatisticsEnabled { get; set; } = true;
        public Guid License { get; set; } = Guid.NewGuid();
    }
}