using System.Collections.Generic;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetReadControllerConfiguration : IFromTypeOptions
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Assembly { get; set; }
        public List<string> ReplaceName { get; set; }
        public List<string> ReplaceWithName { get; set; }
    }
}
