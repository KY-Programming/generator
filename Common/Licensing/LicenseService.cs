using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KY.Core;
using KY.Generator.Settings;
using Newtonsoft.Json;

namespace KY.Generator.Licensing
{
    public class LicenseService
    {
        private readonly GlobalSettingsService globalSettingsService;
        private readonly ManualResetEvent waitForCheck = new(false);
        private string license;

        public LicenseService(GlobalSettingsService globalSettingsService)
        {
            this.globalSettingsService = globalSettingsService;
        }

        public async void Check()
        {
            try
            {
                this.license = await this.SendCommand<string>($"{this.globalSettingsService.Read().License}/check");
                this.waitForCheck.Set();
            }
            catch (Exception exception)
            {
                Logger.Warning(exception.Message + Environment.NewLine + exception.StackTrace);
            }
        }

        public string Get()
        {
            return this.license;
        }

        /// <summary>Blocks the current thread until the license is checked, using a <see cref="T:System.TimeSpan" /> to specify a optional timeout.</summary>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a null to wait indefinitely.</param>
        /// <returns>
        /// <see langword="true" /> if the was checked successful; otherwise, <see langword="false" />.</returns>
        public bool Wait(TimeSpan? timeout = null)
        {
            return timeout.HasValue ? this.waitForCheck.WaitOne(timeout.Value) : this.waitForCheck.WaitOne();
        }

        private async Task<T> SendCommand<T>(string command, string query = "")
        {
// #if DEBUG
//             string baseUri = "http://localhost:8087/api/v1/license";
// #else
            string baseUri = "https://generator.ky-programming.de/api/v1/license";
// #endif
            HttpWebRequest request = WebRequest.CreateHttp($"{baseUri}/{command}?{query}");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();
            using Stream responseStream = response.GetResponseStream();
            string responseString = await responseStream.ReadStringAsync();
            if (responseString.StartsWith("[") || responseString.StartsWith("{") || responseString.StartsWith("\""))
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            Logger.Warning(responseString);
            throw new InvalidOperationException("Can not parse the response. No valid json");
        }
    }
}
