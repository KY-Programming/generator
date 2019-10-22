using KY.Generator.AspDotNet.Templates;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetCoreWriteConfiguration : AspDotNetWriteConfiguration
    {
        public AspDotNetCoreWriteConfiguration()
        {
            this.Template = new DotNetCoreTemplate();
        }
    }
}