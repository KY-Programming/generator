using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenerateWithRetryAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get
            {
                return new[]
                       {
                           new AttributeCommandConfiguration("angular-service", this.Parameters)
                       };
            }
        }

        private List<string> Parameters
        {
            get
            {
                List<string> parameter = new List<string>();
                if (this.EndlessTries)
                {
                    parameter.Add($"-endlessTries");
                }
                if (this.SleepBetweenRetries?.Length > 0)
                {
                    parameter.Add($"-timeouts={string.Join(",", this.SleepBetweenRetries)}");
                }
                return parameter;
            }
        }

        public bool EndlessTries { get; }
        public int[] SleepBetweenRetries { get; }

        public GenerateWithRetryAttribute(params int[] sleepBetweenRetries)
        {
            this.SleepBetweenRetries = sleepBetweenRetries;
        }

        public GenerateWithRetryAttribute(bool endlessTries, params int[] sleepBetweenRetries)
        {
            this.EndlessTries = endlessTries;
            this.SleepBetweenRetries = sleepBetweenRetries;
        }
    }
}