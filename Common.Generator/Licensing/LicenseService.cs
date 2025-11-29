using System.Diagnostics;
using System.Net;
using KY.Core;
using KY.Core.Extension;
using KY.Generator.Settings;
using KY.Generator.Statistics;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

public interface ILicenseService
{
    bool IsValid { get; }

    Task<bool> Has(string feature);
}

internal class LicenseService : ILicenseService
{
    private readonly GlobalSettingsService globalSettingsService;
    private readonly GlobalLicenseService globalLicenseService;
    private readonly StatisticsService statisticsService;
    private readonly ManualResetEvent waitForCheck = new(false);
    private SignedLicense? localLicense;
    private bool isChecking;
    private bool isChecked;
    public bool IsValid { get; private set; }
    public DateTime ValidUntil { get; private set; }
    public IReadOnlyList<string> Features { get; private set; } = [];
    public IReadOnlyList<Message> Messages { get; private set; } = [];

    public LicenseService(GlobalSettingsService globalSettingsService, GlobalLicenseService globalLicenseService, StatisticsService statisticsService)
    {
        this.globalSettingsService = globalSettingsService;
        this.globalLicenseService = globalLicenseService;
        this.statisticsService = statisticsService;
    }

    public void Check()
    {
        if (this.isChecking || this.isChecked)
        {
            return;
        }
        this.isChecking = true;
        Task.Factory.StartNew(async () =>
        {
            try
            {
                SignedLicense? license;
                Guid licenseId;
                if (this.localLicense != null)
                {
                    license = this.localLicense;
                    licenseId = this.localLicense.License?.Id ?? Guid.Empty;
                }
                else
                {
                    licenseId = this.globalSettingsService.Read().License;
                    license = this.globalLicenseService.Read();
                }
                if (license.License == null || license.License.Id != licenseId || (license.License.ValidUntil.Date - DateTime.Today).TotalDays < 7 || !license.Validate())
                {
                    license = await this.SendCommand<SignedLicense>($"{licenseId}/check");
                    if (license == null)
                    {
                        return;
                    }
                    this.globalLicenseService.Set(license);
                }
                this.CheckLicense(license);
            }
            catch (Exception exception)
            {
                Logger.Warning(exception.Message + Environment.NewLine + exception.StackTrace);
                this.CheckLicense(this.globalLicenseService.Read());
            }
            finally
            {
                this.isChecking = false;
            }
        });
    }

    /// <summary>Blocks the current thread until the license is checked or when a cached license is available kills the check.</summary>
    public void WaitOrKill()
    {
        if (this.isChecked)
        {
            return;
        }
        this.CheckLicense(this.globalLicenseService.Read());
        // If the cached license is valid, we can terminate faster
        if (this.IsValid)
        {
            Logger.Trace($"License check skipped. Stored license is valid until {this.ValidUntil.ToShortDateString()}");
        }
        else
        {
            Logger.Trace("Waiting for license check...");
            Stopwatch stopwatch = new();
            stopwatch.Start();
            this.waitForCheck.WaitOne();
            stopwatch.Stop();
            Logger.Trace($"License check finished in {stopwatch.FormattedElapsed()}");
        }
    }

    private void CheckLicense(SignedLicense signedLicense)
    {
        if (signedLicense.IsEmpty)
        {
            return;
        }
        this.IsValid = signedLicense.Validate();
        this.ValidUntil = signedLicense.License?.ValidUntil ?? DateTime.MinValue;
        this.Features = signedLicense.License?.Features ?? [];
        this.Messages = signedLicense.License?.Messages ?? [];
        this.isChecked = true;
        this.waitForCheck.Set();
        this.statisticsService.Data.License = signedLicense.License?.Id ?? Guid.Empty;
    }

    private async Task<T?> SendCommand<T>(string command, string query = "")
    {
#if DEBUG
        string baseUri = "http://localhost:8003/api/v4/license";
#else
        string baseUri = "https://generator.ky-programming.de/api/v4/license";
#endif
        HttpWebRequest request = WebRequest.CreateHttp($"{baseUri}/{command}?{query}");
        request.Method = WebRequestMethods.Http.Get;
        request.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
        WebResponse response = await request.GetResponseAsync();
        using Stream? responseStream = response.GetResponseStream();
        string? responseString = await responseStream?.ReadStringAsync();
        if (responseString != null && (responseString.StartsWith("[") || responseString.StartsWith("{") || responseString.StartsWith("\"")))
        {
            return JsonConvert.DeserializeObject<T>(responseString);
        }
        Logger.Warning(responseString);
        throw new InvalidOperationException("Can not parse the response. No valid json");
    }

    public void ShowMessages()
    {
        foreach (Message message in this.Messages)
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

    public async Task<bool> Has(string feature)
    {
        await this.waitForCheck.WaitOneAsync();
        return this.IsValid && this.Features.Contains(feature);
    }

    public void Set(string certificate)
    {
        this.localLicense = SignedLicense.FromCertificate(certificate);
        if (this.localLicense != null)
        {
            this.CheckLicense(this.localLicense);
        }
    }
}
