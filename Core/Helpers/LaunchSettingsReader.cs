using KY.Core.DataAccess;
using Newtonsoft.Json;

namespace KY.Generator
{
    public class LaunchSettingsReader
    {
        public string ReadApplicationUrl(string path)
        {
            return this.Read(path)?.IISSettings?.IISExpress?.ApplicationUrl;
        }

        public LaunchSettings Read(string path)
        {
            return JsonConvert.DeserializeObject<LaunchSettings>(FileSystem.ReadAllText(path));
        }
    }

    public class LaunchSettings
    {
        [JsonProperty("iisSettings")]
        public IISSettings IISSettings { get; set; }
    }

    public class IISSettings
    {
        [JsonProperty("iisExpress")]
        public IISExpress IISExpress { get; set; }
    }

    public class IISExpress
    {
        [JsonProperty("applicationUrl")]
        public string ApplicationUrl { get; set; }
    }
}