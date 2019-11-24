using System.Collections.Generic;
using System.Linq;
using CreateModule.Configurations;
using KY.Core;
using KY.Generator;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace CreateModule.Writers
{
    internal class DemoModelWriter : Codeable
    {
        public void Write(DemoWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            foreach (ModelTransferObject model in transferObjects.OfType<ModelTransferObject>())
            {
                Logger.Trace($"Write {model.Name}...");
                ClassTemplate classTemplate = files.AddFile(configuration.RelativePath, configuration.AddHeader)
                                                   .AddNamespace(model.Namespace)
                                                   .AddClass(model.Name)
                                                   .FormatName(configuration);
                foreach (PropertyTransferObject property in model.Properties)
                {
                    classTemplate.AddProperty(property.Name, property.Type.ToTemplate());
                }
                if (configuration.Test2)
                {
                    classTemplate.WithUsing("System", null, null)
                                 .AddMethod("Test2", Code.Void())
                                 .Code.AddLine(Code.Static(Code.Type("Console")).Method("WriteLine", Code.String("Hello World!")).Close());
                }
            }
        }
    }
}