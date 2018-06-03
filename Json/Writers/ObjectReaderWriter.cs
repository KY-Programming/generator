using System;
using System.IO;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Json.Configuration;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Json.Writers
{
    public class ObjectReaderWriter
    {
        private readonly JsonGenerator generator;

        public ObjectReaderWriter(JsonGenerator generator)
        {
            this.generator = generator;
        }

        public void Write(JsonConfiguration configuration)
        {
            if (configuration.ToObject == null)
            {
                throw new InvalidOperationException("Can not create JsonObjectReader without Object. Add in Settings Json.ToObject property");
            }
            if (configuration.Language != Csharp.Code.Language)
            {
                throw new InvalidOperationException($"Can not generate JsonReader ({configuration.WithReader.Name}) for language {configuration.Language}");
            }
            string objectName = Path.GetFileNameWithoutExtension(configuration.ToObject.Source);
            string name = configuration.WithReader.Name ?? objectName + "Reader";
            ClassTemplate classTemplate = this.generator.Files.AddFile(configuration.WithReader.RelativePath)
                                              .AddNamespace(configuration.WithReader.Namespace)
                                              .AddClass(name)
                                              .WithUsing("Newtonsoft.Json")
                                              .WithUsing("Newtonsoft.Json.Linq")
                                              .WithUsing("System.IO");

            foreach (ClassTemplate writtenObject in configuration.WithReader.WrittenObjects)
            {
                classTemplate.AddProperty(writtenObject.Name, writtenObject.ToType());
                classTemplate.AddUsing(writtenObject.ToUsing());
            }

            classTemplate.AddMethod("Load", classTemplate.ToType())
                         .WithParameter(Code.Type("string"), "fileName")
                         .Static()
                         .Code.AddLine(Code.Return(Code.This().Method("Parse", Code.Local("File").Method("ReadAllText", Code.Local("configFile")))));

            classTemplate.AddMethod("Parse", classTemplate.ToType())
                         .WithParameter(Code.Type("string"), "json")
                         .Static()
                         .Code.AddLine(Code.Return(Code.Local("JsonConvert").GenericMethod("DeserializeObject", classTemplate.ToType(), Code.Local("json"))));
        }
    }
}