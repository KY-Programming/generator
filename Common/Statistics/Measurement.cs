using System;
using System.Diagnostics;

namespace KY.Generator.Statistics
{
    public class Measurement
    {
        private Stopwatch StopWatch { get; } = new();
        public TimeSpan Elapsed => this.StopWatch.Elapsed;

        public Measurement()
        {
            this.StopWatch.Reset();
            this.StopWatch.Start();
        }

        public void Stop()
        {
            this.StopWatch.Stop();
        }

    }
}