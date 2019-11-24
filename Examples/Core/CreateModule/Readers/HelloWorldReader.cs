using System.Collections.Generic;
using CreateModule.Configurations;
using KY.Core;
using KY.Generator.Transfer;

namespace CreateModule.Readers
{
    public class HelloWorldReader
    {
        public void Read(DemoReadConfiguration configuration, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Read {configuration.HelloWorld.Name}...");
            // Read something here and create your transfer objects
            ModelTransferObject model = new ModelTransferObject { Name = configuration.HelloWorld.Name, Namespace = "Test" };
            if (configuration.HelloWorld.Test1)
            {
                model.Properties.Add(new PropertyTransferObject { Name = "Test1", Type = new TypeTransferObject { Name = "string" } });
            }
            transferObjects.Add(model);
        }
    }
}