using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.Angular.Commands
{
    internal class WriteAngularCommand : IConfigurationCommand
    {
        private readonly IDependencyResolver resolver;

        public WriteAngularCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Write angular...");
            AngularWriteConfiguration configuration = (AngularWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.Service != null)
            {
                this.resolver.Create<AngularServiceWriter>().Write(configuration, transferObjects, files);
            }
            if (configuration.WriteModels)
            {
                this.resolver.Create<AngularModelWriter>().Write(configuration, transferObjects, files);
            }
            files.Write(configuration);
            return true;
        }
    }
}