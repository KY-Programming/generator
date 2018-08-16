using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.AspDotNet.Templates;
using KY.Generator.Configuration;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.AspDotNet
{
    internal class GeneratorGenerator : IGenerator
    {
        private readonly List<ITemplate> templates;

        public List<FileTemplate> Files { get; }

        public GeneratorGenerator()
        {
            this.Files = new List<FileTemplate>();
            this.templates = new List<ITemplate>();
            this.templates.Add(new DotNetFrameworkTemplate());
            this.templates.Add(new DotNetCoreTemplate());
        }

        public void Generate(ConfigurationBase configuration)
        {
            this.Files.Clear();
            GeneratorConfiguration generator = configuration as GeneratorConfiguration;
            if (generator == null)
            {
                return;
            }
            ITemplate template = this.templates.FirstOrDefault(x => x.Name == generator.Framework) ?? this.templates.First();
            if (generator.Controller != null)
            {
                if (configuration.Language != Csharp.Code.Language)
                {
                    throw new InvalidOperationException($"Can not generate Generator.Controller for language {configuration.Language}");
                }
                ClassTemplate classTemplate = this.Files.AddFile(generator.Controller.RelativePath)
                                                  .AddNamespace(generator.Controller.Namespace)
                                                  .AddClass("GeneratorController", Code.Type("Controller"))
                                                  .WithUsing("System")
                                                  .WithUsing("System.Linq")
                                                  .WithUsing("KY.Generator")
                                                  .WithUsing("KY.Generator.Output");

                classTemplate.Usings.AddRange(template.Usings);

                if (template.UseOwnCache)
                {
                    GenericTypeTemplate type = Code.Generic("Dictionary", Code.Type("string"), Code.Type("MemoryOutput"));
                    classTemplate.AddField("cache", type)
                                 .Static()
                                 .DefaultValue = Code.New(type);
                }

                MethodTemplate createMethode = classTemplate.AddMethod("Create", Code.Type("Guid"))
                                                            .WithParameter(Code.Type("string"), "configuration");
                if (!template.ValidateInput)
                {
                    createMethode.WithAttribute("ValidateInput", Code.Local("false"));
                }

                MethodTemplate commandMethode = classTemplate.AddMethod("Command", Code.Type("Guid"))
                                                             .WithParameter(Code.Type("string"), "command");

                MultilineCodeFragment createCode = createMethode.Code;
                createCode.AddLine(Code.Declare(Code.Type("Guid"), "id", Code.Local("Guid").Method("NewGuid")))
                          .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                          .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                          .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")));
                MultilineCodeFragment commandCode = commandMethode.Code;
                commandCode.AddLine(Code.Declare(Code.Type("Guid"), "id", Code.Local("Guid").Method("NewGuid")))
                          .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                          .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                          .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")));
                foreach (string nameSpace in generator.Controller.Usings)
                {
                    classTemplate.AddUsing(nameSpace);
                }
                foreach (string moduleType in generator.Controller.PreloadModules)
                {
                    createCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)));
                    commandCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)));
                }
                foreach (GeneratorConfigurationConfigureModule configure in generator.Controller.Configures)
                {
                    createCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Csharp.Code.Csharp("x." + configure.Action))));
                    commandCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Csharp.Code.Csharp("x." + configure.Action))));
                }
                createCode.AddLine(Code.Local("generator").Method("ParseConfiguration", Code.Local("configuration")))
                          .AddLine(Code.Local("generator").Method("Run"))
                          .AddBlankLine();
                commandCode.AddLine(Code.Local("generator").Method("ParseCommand", Code.Local("command")))
                          .AddLine(Code.Local("generator").Method("Run"))
                          .AddBlankLine();
                if (template.UseOwnCache)
                {
                    createCode.AddLine(Code.Local("cache").Index(Code.Local("id").Method("ToString")).Assign(Code.Local("output")));
                    commandCode.AddLine(Code.Local("cache").Index(Code.Local("id").Method("ToString")).Assign(Code.Local("output")));
                }
                else
                {
                    createCode.AddLine(Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id").Method("ToString")).Assign(Code.Local("output")));
                    commandCode.AddLine(Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id").Method("ToString")).Assign(Code.Local("output")));
                }
                createCode.AddLine(Code.Return(Code.Local("id")));
                commandCode.AddLine(Code.Return(Code.Local("id")));

                ChainedCodeFragment getFromCacheForFilesFragment = template.UseOwnCache
                                                                       ? (ChainedCodeFragment)Code.Local("cache")
                                                                       : Code.This().Property("HttpContext").Property("Cache");
                classTemplate.AddMethod("GetFiles", Code.Type("string"))
                             .WithParameter(Code.Type("string"), "id")
                             .Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                             .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFilesFragment.Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                             .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null(),
                                                                Code.Null(),
                                                                Code.Local("string").Method("Join", Code.Local("Environment").Property("NewLine"),
                                                                                            Code.Local("output").Property("Files").Method("Select", Code.Lambda("x", Code.Local("x").Property("Key")))))));

                ChainedCodeFragment getFromCacheForFileFragment = template.UseOwnCache
                                                                      ? (ChainedCodeFragment)Code.Local("cache")
                                                                      : Code.This().Property("HttpContext").Property("Cache");
                classTemplate.AddMethod("GetFile", Code.Type("string"))
                             .WithParameter(Code.Type("string"), "id")
                             .WithParameter(Code.Type("string"), "path")
                             .Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                             .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFileFragment.Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                             .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null().Or().Not().Local("output").Property("Files").Method("ContainsKey", Code.Local("path")),
                                                                Code.Null(),
                                                                Code.Local("output").Property("Files").Index(Code.Local("path")))));
            }
        }
    }
}