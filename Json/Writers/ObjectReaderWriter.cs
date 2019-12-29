using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Configurations;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;

namespace KY.Generator.Json.Writers
{
    public class ObjectReaderWriter : Codeable
    {
        public IEnumerable<FileTemplate> Write(JsonWriteConfiguration configuration, List<ITransferObject> transferObjects)
        {
            ModelTransferObject model = transferObjects.OfType<ModelTransferObject>().FirstOrDefault();
            if (model == null)
            {
                throw new InvalidOperationException("Can not create JsonObjectReader without object. Add in settings a read configuration");
            }
            if (!configuration.Language.IsCsharp())
            {
                throw new InvalidOperationException($"Can not generate JsonReader ({configuration.Reader.Name}) for language {configuration.Language}");
            }
            Logger.Trace("Write JsonReader...");
            string className = configuration.Reader.Name ?? model.Name + "Reader";
            FileTemplate fileTemplate = new FileTemplate(configuration.Reader.RelativePath, configuration.AddHeader, configuration.CheckOnOverwrite);
            ClassTemplate classTemplate = fileTemplate.AddNamespace(configuration.Reader.Namespace ?? model.Namespace ?? configuration.Object.Namespace)
                                                      .AddClass(className)
                                                      .FormatName(configuration);
            WriteReader(classTemplate, model, configuration.FormatNames);

            yield return fileTemplate;
        }

        internal static void WriteReader(ClassTemplate classTemplate, ModelTransferObject model, bool formatNames)
        {
            TypeTemplate objectType = Code.Type(model.Name, model.Namespace);
            if (model.Namespace != classTemplate.Namespace.Name && model.Namespace != null)
            {
                classTemplate.AddUsing(model.Namespace);
            }
            classTemplate.WithUsing("Newtonsoft.Json")
                         //.WithUsing("Newtonsoft.Json.Linq")
                         .WithUsing("System.IO");

            classTemplate.AddMethod("Load", objectType)
                         .FormatName(model.Language, formatNames)
                         .WithParameter(Code.Type("string"), "fileName")
                         .Static()
                         .Code.AddLine(Code.Return(Code.Method("Parse", Code.Local("File").Method("ReadAllText", Code.Local("fileName")))));

            classTemplate.AddMethod("Parse", objectType)
                         .FormatName(model.Language, formatNames)
                         .WithParameter(Code.Type("string"), "json")
                         .Static()
                         .Code.AddLine(Code.Return(Code.Local("JsonConvert").GenericMethod("DeserializeObject", objectType, Code.Local("json"))));
        }
    }
}