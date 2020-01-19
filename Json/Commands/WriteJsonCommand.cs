using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Writers;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.Json.Commands
{
    internal class WriteJsonCommand : IConfigurationCommand
    {
        private readonly IDependencyResolver resolver;

        public WriteJsonCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Write json...");
            JsonWriteConfiguration configuration = (JsonWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.Object != null)
            {
                this.resolver.Create<ObjectWriter>().Write(configuration, transferObjects).ForEach(files.Add);
            }
            if (configuration.Reader != null)
            {
                this.resolver.Create<ObjectReaderWriter>().Write(configuration, transferObjects).ForEach(files.Add);
            }
            if (configuration.Object == null && configuration.Reader == null)
            {
                Logger.Warning("Json configuration has no model and no reader property. Set at least one of them to start generation");
            }
            files.Write(configuration);
            return true;
        }
    }
}