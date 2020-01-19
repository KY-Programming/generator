using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Commands
{
    public class ReadAspDotNetCommand : IConfigurationCommand
    {
        private readonly IDependencyResolver resolver;

        public ReadAspDotNetCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Read ASP.net...");
            AspDotNetReadConfiguration configuration = (AspDotNetReadConfiguration)configurationBase;
            if (configuration.Controller != null)
            {
                this.resolver.Create<AspDotNetControllerReader>().Read(configuration, transferObjects);
            }
            return true;
        }
    }
}