using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Templates;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.AspDotNet.Writers
{
    internal class AspDotNetGeneratorControllerWriter : Codeable
    {
        private readonly List<ITemplate> templates;

        public AspDotNetGeneratorControllerWriter()
        {
            this.templates = new List<ITemplate>();
            this.templates.Add(new DotNetFrameworkTemplate());
            this.templates.Add(new DotNetCoreTemplate());
        }

        public void Write(AspDotNetWriteConfiguration configuration, IOutput output)
        {
            Logger.Trace("Generate generator controller for ASP.net...");
            if (!configuration.Language.IsCsharp())
            {
                throw new InvalidOperationException($"Can not generate ASP.net Controller for language {configuration.Language?.Name ?? "Empty"}");
            }

            ITemplate template = this.templates.FirstOrDefault(x => x.Name == configuration.Framework) ?? this.templates.First();
            if (configuration.Standalone)
            {
                throw new InvalidOperationException("Can not generate Generator.Controller with KY.Generator.CLI.Standalone use KY.Generator.CLI instead");
            }
            List<FileTemplate> files = new List<FileTemplate>();
            ClassTemplate classTemplate = files.AddFile(configuration.Controller.RelativePath)
                                              .AddNamespace(configuration.Controller.Namespace)
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
                             .FormatName(configuration.Language, configuration.FormatNames)
                             .Static()
                             .Readonly()
                             .DefaultValue = Code.New(type);
            }

            MethodTemplate createMethod = classTemplate.AddMethod("Create", Code.Type("string"))
                                                       .FormatName(configuration.Language, configuration.FormatNames)
                                                       .WithParameter(Code.Type("string"), "configuration");
            if (!template.ValidateInput)
            {
                createMethod.WithAttribute("ValidateInput", Code.Local("false"));
            }

            MethodTemplate commandMethod = classTemplate.AddMethod("Command", Code.Type("string"))
                                                        .FormatName(configuration.Language, configuration.FormatNames)
                                                        .WithParameter(Code.Type("string"), "command");

            MultilineCodeFragment createCode = createMethod.Code;
            createCode.AddLine(Code.Declare(Code.Type("string"), "id", Code.Local("Guid").Method("NewGuid").Method("ToString")))
                      .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                      .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                      .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")));
            MultilineCodeFragment commandCode = commandMethod.Code;
            commandCode.AddLine(Code.Declare(Code.Type("string"), "id", Code.Local("Guid").Method("NewGuid").Method("ToString")))
                       .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                       .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                       .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")));
            foreach (string nameSpace in configuration.Controller.Usings)
            {
                classTemplate.AddUsing(nameSpace);
            }
            foreach (string moduleType in configuration.Controller.PreloadModules)
            {
                createCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)));
                commandCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)));
            }
            foreach (AspDotNetControllerConfigureModule configure in configuration.Controller.Configures)
            {
                createCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Code.Csharp("x." + configure.Action))));
                commandCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Code.Csharp("x." + configure.Action))));
            }
            createCode.AddLine(Code.Local("generator").Method("ParseConfiguration", Code.Local("configuration")))
                      .AddLine(Code.Local("generator").Method("Run"))
                      .AddBlankLine();
            commandCode.AddLine(Code.Local("generator").Method("ParseCommand", Code.Local("command")))
                       .AddLine(Code.Local("generator").Method("Run"))
                       .AddBlankLine();
            if (template.UseOwnCache)
            {
                createCode.AddLine(Code.Local("cache").Index(Code.Local("id")).Assign(Code.Local("output")));
                commandCode.AddLine(Code.Local("cache").Index(Code.Local("id")).Assign(Code.Local("output")));
            }
            else
            {
                createCode.AddLine(Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id")).Assign(Code.Local("output")));
                commandCode.AddLine(Code.This().Property("HttpContext").Property("Cache").Index(Code.Local("id")).Assign(Code.Local("output")));
            }
            createCode.AddLine(Code.Return(Code.Local("id")));
            commandCode.AddLine(Code.Return(Code.Local("id")));

            ChainedCodeFragment getFromCacheForFilesFragment = template.UseOwnCache
                                                                   ? (ChainedCodeFragment)Code.Local("cache")
                                                                   : Code.This().Property("HttpContext").Property("Cache");
            MethodTemplate getFilesMethod = classTemplate.AddMethod("GetFiles", Code.Type("string"))
                                                         .FormatName(configuration.Language, configuration.FormatNames)
                                                         .WithParameter(Code.Type("string"), "id");
            getFilesMethod.Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                          .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFilesFragment.Index(Code.Local("id"))))
                          .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null(),
                                                             Code.Null(),
                                                             Code.Local("string").Method("Join", Code.Local("Environment").Property("NewLine"),
                                                                                         Code.Local("output").Property("Files").Method("Select", Code.Lambda("x", Code.Local("x").Property("Key")))))));

            ChainedCodeFragment getFromCacheForFileFragment = template.UseOwnCache
                                                                  ? (ChainedCodeFragment)Code.Local("cache")
                                                                  : Code.This().Property("HttpContext").Property("Cache");
            MethodTemplate getFileMethod = classTemplate.AddMethod("GetFile", Code.Type("string"))
                                                        .FormatName(configuration.Language, configuration.FormatNames)
                                                        .WithParameter(Code.Type("string"), "id")
                                                        .WithParameter(Code.Type("string"), "path");
            getFileMethod.Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                         .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFileFragment.Index(Code.Local("id"))))
                         .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null().Or().Not().Local("output").Property("Files").Method("ContainsKey", Code.Local("path")),
                                                            Code.Null(),
                                                            Code.Local("output").Property("Files").Index(Code.Local("path")))));
            MethodTemplate availableMethod = classTemplate.AddMethod("Available", Code.Type("bool"))
                                                          .FormatName(configuration.Language, configuration.FormatNames);
            availableMethod.Code.AddLine(Code.Return(Code.Local("true")));

            if (template.UseAttributes)
            {
                classTemplate.WithUsing("Microsoft.AspNetCore.Mvc")
                             .WithAttribute("Route", Code.String("[controller]"))
                             .WithAttribute("Route", Code.String("api/v1/[controller]"));
                createMethod.WithAttribute("HttpPost", Code.String("[action]"));
                commandMethod.WithAttribute("HttpPost", Code.String("[action]"));
                getFilesMethod.WithAttribute("HttpPost", Code.String("[action]"));
                getFileMethod.WithAttribute("HttpPost", Code.String("[action]"));
                availableMethod.WithAttribute("HttpGet", Code.String("[action]"));
            }
            
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}