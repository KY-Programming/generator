using KY.Generator.Configuration;
using KY.Generator.Csharp.Languages;

namespace KY.Generator.AspDotNet.Configurations
{
    internal class AspDotNetWriteConfiguration : ConfigurationBase
    {
        public bool FormatNames { get; set; }
        public AspDotNetGeneratorControllerConfiguration Controller { get; set; }

        public AspDotNetWriteConfiguration()
        {
            this.Language = CsharpLanguage.Instance;
            this.FormatNames = true;
        }
    }
}