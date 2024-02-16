using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using KY.Core;
using KY.Generator.Settings;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

internal class LicenseService
{
    private readonly GlobalSettingsService globalSettingsService;
    private readonly GlobalLicenseService globalLicenseService;
    private readonly ManualResetEvent waitForCheck = new(false);
    public bool IsValid { get; private set; }
    public DateTime ValidUntil { get; private set; }

    public LicenseService(GlobalSettingsService globalSettingsService, GlobalLicenseService globalLicenseService)
    {
        this.globalSettingsService = globalSettingsService;
        this.globalLicenseService = globalLicenseService;
    }

    public void Check()
    {
        Task.Factory.StartNew(async () =>
        {
            try
            {
                Guid licenseId = this.globalSettingsService.Read()?.License ?? Guid.Empty;
                SignedLicense license = this.globalLicenseService.Read();
                if (license.License.Id == licenseId && (license.License.ValidUntil.Date - DateTime.Today).TotalDays >= 7 && license.Validate())
                {
                    this.globalLicenseService.Set(license);
                }
                else
                {
                    license = await this.SendCommand<SignedLicense>($"{licenseId}/check");
                    this.globalLicenseService.Set(license);
                }
                this.CheckLicense(license);
            }
            catch (Exception exception)
            {
                Logger.Warning(exception.Message + Environment.NewLine + exception.StackTrace);
                this.CheckLicense(this.globalLicenseService.Read());
            }
            this.waitForCheck.Set();
        });
    }

    /// <summary>Blocks the current thread until the license is checked or when a cached license is available kills the check.</summary>
    public void WaitOrKill()
    {
        this.CheckLicense(this.globalLicenseService.Read());
        // If the cached license is valid, we can terminate faster
        if (this.IsValid)
        {
            Logger.Trace($"License check skipped. Stored license is valid until {this.ValidUntil}");
            this.waitForCheck.Set();
        }
        else
        {
            this.waitForCheck.WaitOne();
        }
    }

    private void CheckLicense(SignedLicense signedLicense)
    {
        this.IsValid = signedLicense.Validate();
        this.ValidUntil = signedLicense.License?.ValidUntil ?? DateTime.MinValue;
    }

    private async Task<T> SendCommand<T>(string command, string query = "")
    {
#if DEBUG
        string baseUri = "http://localhost:8003/api/v3/license";
#else
            string baseUri = "https://generator.ky-programming.de/api/v3/license";
#endif
        HttpWebRequest request = WebRequest.CreateHttp($"{baseUri}/{command}?{query}");
        request.Method = WebRequestMethods.Http.Get;
        request.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
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

    public void ShowMessages()
    {
        SignedLicense signedLicense = this.globalLicenseService.Read();
        if (signedLicense.License?.Messages == null)
        {
            return;
        }
        foreach (Message message in signedLicense.License.Messages)
        {
            switch (message.Type)
            {
                case MessageType.Info:
                    Logger.Trace(message.Text);
                    break;
                case MessageType.Warning:
                    Logger.Warning(message.Text);
                    break;
                default:
                    Logger.Error(message.Text);
                    break;
            }
        }
    }
}
