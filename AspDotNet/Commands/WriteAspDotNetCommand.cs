using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Writers;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Commands
{
    public class WriteAspDotNetCommand : IConfigurationCommand
    {
        private readonly IDependencyResolver resolver;

        public WriteAspDotNetCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Write ASP.net...");
            AspDotNetWriteConfiguration configuration = (AspDotNetWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.GeneratorController != null)
            {
                this.resolver.Create<AspDotNetGeneratorControllerWriter>().Write(configuration, files);
            }
            if (configuration.Controllers.Count > 0)
            {
                this.resolver.Create<AspDotNetEntityControllerWriter>().Write(configuration, transferObjects, files);
            }
            files.Write(configuration);
            return true;
        }
    }
}