using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

/// <summary>
/// Caches the signed license that the api hands out for a license id.
/// <para>
/// The cache is kept per id, because a repository can name a license of its own in its <c>ky-generator.json</c>. With
/// a single cache slot every build that switches between such a repository and one without would find the id of the
/// cached license mismatching and ask the api again - a request with a five second timeout on every build
/// </para>
/// </summary>
internal class GlobalLicenseService
{
    /// <summary>
    /// The cache of the versions that only knew the license of the machine. It is read once, for the id it was
    /// written for, so an update does not force one more round trip to the api
    /// </summary>
    private const string LegacyFileName = "global.license.json";

    private readonly Dictionary<Guid, SignedLicense> cache = new();
    private readonly IEnvironment environment;

    public GlobalLicenseService(IEnvironment environment)
    {
        this.environment = environment;
    }

    public SignedLicense Read(Guid licenseId)
    {
        lock (this)
        {
            if (this.cache.TryGetValue(licenseId, out SignedLicense cached))
            {
                return cached;
            }
            SignedLicense license = this.ReadFile(this.GetFileName(licenseId))
                                    ?? this.ReadLegacyFile(licenseId)
                                    ?? new SignedLicense();
            this.cache[licenseId] = license;
            return license;
        }
    }

    public void Set(Guid licenseId, SignedLicense license)
    {
        lock (this)
        {
            this.cache[licenseId] = license;
            FileSystem.WriteAllText(this.GetFileName(licenseId), JsonConvert.SerializeObject(license));
        }
    }

    private string GetFileName(Guid licenseId)
    {
        return FileSystem.Combine(this.environment.ApplicationData, $"global.license.{licenseId}.json");
    }

    private SignedLicense? ReadFile(string fileName)
    {
        if (!FileSystem.FileExists(fileName))
        {
            return null;
        }
        try
        {
            return JsonConvert.DeserializeObject<SignedLicense>(FileSystem.ReadAllText(fileName));
        }
        catch (Exception exception)
        {
            Logger.Warning($"Could not read {fileName}." + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace);
            return null;
        }
    }

    /// <summary>
    /// Takes the license out of the cache of an older version, but only if it is the one that was asked for
    /// </summary>
    private SignedLicense? ReadLegacyFile(Guid licenseId)
    {
        SignedLicense? license = this.ReadFile(FileSystem.Combine(this.environment.ApplicationData, LegacyFileName));
        if (license?.License == null || license.License.Id != licenseId)
        {
            return null;
        }
        this.Set(licenseId, license);
        return license;
    }
}
