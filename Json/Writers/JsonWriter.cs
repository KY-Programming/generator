using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Json.Configurations;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Json.Writers
{
    internal class JsonWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public JsonWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            JsonWriteConfiguration configuration = (JsonWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            this.resolver.Create<ObjectWriter>().Write(configuration, transferObjects).ForEach(files.Add);
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}