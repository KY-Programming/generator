using System;
using System.Net.Http;
using System.Threading.Tasks;
using KY.Core;

namespace KY.Generator.Watchdog.Watchdogs
{
    internal class HttpWatchdog
    {
        private readonly string url;
        private readonly int tries;
        private readonly TimeSpan delay;
        private readonly TimeSpan sleep;
        private readonly TimeSpan timeout;

        public HttpWatchdog(string url, int tries, TimeSpan delay, TimeSpan sleep, TimeSpan timeout)
        {
            this.url = url;
            this.tries = tries;
            this.delay = delay;
            this.sleep = sleep;
            this.timeout = timeout;
        }

        public async Task<bool> WaitAsync()
        {
            DateTime endOfLife = DateTime.Now.Add(this.timeout);
            Logger.Trace($"Wait for {this.url}...");
            await Task.Delay(this.delay);
            int triesLeft = this.tries;
            HttpClient client = new HttpClient();
            while (triesLeft > 0 || this.tries == 0)
            {
                if (DateTime.Now >= endOfLife)
                {
                    Logger.Trace("Wait failed. End of life reached. Try to increase timeout (-timeout=00:10:00)");
                    return false;
                }
                try
                {
                    HttpResponseMessage response = await client.GetAsync(this.url);
                    if (response.IsSuccessStatusCode)
                    {
                        Logger.Trace("Wait successful");
                        return true;
                    }
                }
                catch (Exception)
                {
                    // Ignore all exception while we wait
                }
                triesLeft--;
                await Task.Delay(this.sleep);
            }
            Logger.Trace("Wait failed. Tries exceeded. Try to increase tries (-tries=1000)");
            return false;
        }
    }
}