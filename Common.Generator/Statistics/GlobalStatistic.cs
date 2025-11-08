using System;
using System.Collections.Generic;

namespace KY.Generator.Statistics
{
    public class GlobalStatistic
    {
        public DateTime Today { get; set; } = DateTime.Today;
        public long TodayLines { get; set; }
        public long TodayFiles { get; set; }
        public long Lines { get; set; }
        public long Files { get; set; }
        public List<Guid> Ids { get; } = new();
    }
}
