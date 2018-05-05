using System;
using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Properties;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.AspDotNet
{
    internal class GeneratorGenerator : IGenerator
    {
        public List<FileTemplate> Files { get; }

        public GeneratorGenerator()
        {
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configuration)
        {
            this.Files.Clear();
            GeneratorConfiguration generator = configuration as GeneratorConfiguration;
            if (generator == null)
            {
                return;
            }
            if (generator.Controller != null)
            {
                if (configuration.Language != Csharp.Language)
                {
                    throw new InvalidOperationException($"Can not generate Generator.Controller for language {configuration.Language}");
                }
                ClassTemplate classTemplate = this.Files.AddFile(generator.Controller.RelativePath)
                                                  .WithHeader(configuration.AddHeader ? Resources.Header : string.Empty)
                                                  .AddNamespace(generator.Controller.Namespace)
                                                  .AddClass("GeneratorController", Code.Type("Controller"))
                                                  .WithUsing("System")
                                                  .WithUsing("System.Linq")
                                                  .WithUsing("System.Web.Mvc")
                                                  .WithUsing("KY.Generator")
                                                  .WithUsing("KY.Generator.Output");

                classTemplate.AddMethod("Create", Code.Type("Guid"))
                             .WithAttribute("ValidateInput", Code.Local("false"))
                             .WithParameter(Code.Type("string"), "configuration")
                             .Code.AddLine(Code.Declare(Code.Type("Guid"), "id", Code.Local("Guid").Method("NewGuid")))
                             .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                             .AddLine(Code.Declare(Code.Type("Generator.Generator"), "generator", Code.New(Code.Type("Generator.Generator"))))
                             .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")))
                             .AddLine(Code.Local("generator").Method("ParseConfiguration", Code.Local("configuration")))
                             .AddLine(Code.Local("generator").Method("Run"))
                             .AddBlankLine()
                             .AddLine(Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id").Method("ToString")).Assign(Code.Local("output")))
                             .AddLine(Code.Return(Code.Local("id")));

                classTemplate.AddMethod("GetFiles", Code.Type("string"))
                             .WithParameter(Code.Type("string"), "id")
                             .Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                             .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                             .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null(),
                                                  Code.Null(),
                                                  Code.Local("string").Method("Join", Code.Local("Environment").Property("NewLine"),
                                                                              Code.Local("output").Property("Files").Method("Select", Code.Lambda("x", Code.Local("x").Property("Key")))))));

                classTemplate.AddMethod("GetFile", Code.Type("string"))
                             .WithParameter(Code.Type("string"), "id")
                             .WithParameter(Code.Type("string"), "path")
                             .Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                             .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                             .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null().Or().Not().Local("output").Property("Files").Method("ContainsKey", Code.Local("path")),
                                                  Code.Null(),
                                                  Code.Local("output").Property("Files").Index(Code.Local("path")))));
            }
        }
    }
}