using System;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator.Licensing;

internal class GlobalLicenseService
{
    private SignedLicense cache;
    private readonly string fileName;

    public GlobalLicenseService(IEnvironment environment)
    {
        this.fileName = FileSystem.Combine(environment.ApplicationData, "global.license.json");
    }

    public SignedLicense Read()
    {
        lock (this)
        {
            if (this.cache == null)
            {
                if (FileSystem.FileExists(this.fileName))
                {
                    try
                    {
                        this.cache = JsonConvert.DeserializeObject<SignedLicense>(FileSystem.ReadAllText(this.fileName));
                    }
                    catch (Exception exception)
                    {
                        Logger.Warning("Could not read global license." + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace);
                        this.cache = null;
                    }
                }
                this.cache ??= new SignedLicense();
            }
            return this.cache;
        }
    }

    public void Write()
    {
        lock (this)
        {
            this.cache.AssertIsNotNull(null, "No data found. Use Read() method first");
            FileSystem.WriteAllText(this.fileName, JsonConvert.SerializeObject(this.cache));
        }
    }

    public void Set(SignedLicense license)
    {
        lock (this)
        {
            this.cache = license;
            this.Write();
        }
    }
}
