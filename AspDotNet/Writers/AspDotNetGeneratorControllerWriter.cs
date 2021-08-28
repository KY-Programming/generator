using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.AspDotNet.Writers
{
    public class AspDotNetGeneratorControllerWriter : Codeable
    {
        private readonly Options options;

        public AspDotNetGeneratorControllerWriter(Options options)
        {
            this.options = options;
        }

        public virtual void Write(AspDotNetWriteConfiguration configuration, List<FileTemplate> files)
        {
            Logger.Trace("Generate generator controller for ASP.net...");
            if (!this.options.Current.Language.IsCsharp())
            {
                throw new InvalidOperationException($"Can not generate ASP.net Controller for language {this.options.Current.Language?.Name ?? "Empty"}. Only Csharp is currently implemented");
            }
            string nameSpace = (configuration.GeneratorController.Namespace ?? configuration.Namespace).AssertIsNotNull(nameof(configuration.Namespace), "asp writer requires a namespace");
            string className = "GeneratorController";
            ClassTemplate classTemplate = files.AddFile(configuration.GeneratorController.RelativePath ?? configuration.RelativePath, this.options.Current)
                                               .WithName(Formatter.FormatFile(className, this.options.Current))
                                               .AddNamespace(nameSpace)
                                               .AddClass(className, Code.Type(configuration.Template.ControllerBase))
                                               .WithUsing("System")
                                               .WithUsing("System.Linq")
                                               .WithUsing("KY.Generator")
                                               .WithUsing("KY.Generator.Output");

            classTemplate.Usings.AddRange(configuration.Template.Usings);

            if (configuration.Template.UseOwnCache)
            {
                GenericTypeTemplate type = Code.Generic("Dictionary", Code.Type("string"), Code.Type("MemoryOutput"));
                classTemplate.AddField("cache", type)
                             // .FormatName(configuration)
                             .Static()
                             .Readonly()
                             .DefaultValue = Code.New(type);
            }

            MethodTemplate createMethod = classTemplate.AddMethod("Create", Code.Type("string"))
                                                       // .FormatName(configuration)
                                                       .WithParameter(Code.Type("string"), "configuration");
            if (!configuration.Template.ValidateInput)
            {
                createMethod.WithAttribute("ValidateInput", Code.Local("false"));
            }

            MethodTemplate commandMethod = classTemplate.AddMethod("Command", Code.Type("string"))
                                                        // .FormatName(configuration)
                                                        .WithParameter(Code.Type("string"), "command");

            MultilineCodeFragment createCode = createMethod.Code;
            createCode.AddLine(Code.Declare(Code.Type("string"), "id", Code.Local("Guid").Method("NewGuid").Method("ToString")))
                      .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                      .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                      .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")).Close());
            MultilineCodeFragment commandCode = commandMethod.Code;
            commandCode.AddLine(Code.Declare(Code.Type("string"), "id", Code.Local("Guid").Method("NewGuid").Method("ToString")))
                       .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", Code.New(Code.Type("MemoryOutput"))))
                       .AddLine(Code.Declare(Code.Type("Generator"), "generator", Code.New(Code.Type("Generator"))))
                       .AddLine(Code.Local("generator").Method("SetOutput", Code.Local("output")).Close());
            foreach (string ns in configuration.GeneratorController.Usings)
            {
                classTemplate.AddUsing(ns);
            }
            foreach (string moduleType in configuration.GeneratorController.PreloadModules)
            {
                createCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)).Close());
                commandCode.AddLine(Code.Local("generator").GenericMethod("PreloadModule", Code.Type(moduleType)).Close());
            }
            foreach (AspDotNetControllerConfigureModule configure in configuration.GeneratorController.Configures)
            {
                createCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Code.Csharp("x." + configure.Action))));
                commandCode.AddLine(Code.Local("generator").Method(configure.Module, Code.Lambda("x", Code.Csharp("x." + configure.Action))));
            }
            createCode.AddLine(Code.Local("generator").Method("ParseConfiguration", Code.Local("configuration")).Close())
                      .AddLine(Code.Local("generator").Method("Run").Close())
                      .AddBlankLine();
            commandCode.AddLine(Code.Local("generator").Method("ParseCommand", Code.Local("command")).Close())
                       .AddLine(Code.Local("generator").Method("Run").Close())
                       .AddBlankLine();
            if (configuration.Template.UseOwnCache)
            {
                createCode.AddLine(Code.Local("cache").Index(Code.Local("id")).Assign(Code.Local("output")).Close());
                commandCode.AddLine(Code.Local("cache").Index(Code.Local("id")).Assign(Code.Local("output")).Close());
            }
            else
            {
                createCode.AddLine(Code.Static(Code.Type("HttpContext")).Property("Current").Property("Cache").Index(Code.Local("id")).Assign(Code.Local("output")).Close());
                commandCode.AddLine(Code.Static(Code.Type("HttpContext")).Property("Current").Property("Cache").Index(Code.Local("id")).Assign(Code.Local("output")).Close());
            }
            createCode.AddLine(Code.Return(Code.Local("id")));
            commandCode.AddLine(Code.Return(Code.Local("id")));

            ChainedCodeFragment getFromCacheForFilesFragment = configuration.Template.UseOwnCache
                                                                   ? (ChainedCodeFragment)Code.Local("cache")
                                                                   : Code.Static(Code.Type("HttpContext")).Property("Current").Property("Cache");
            MethodTemplate getFilesMethod = classTemplate.AddMethod("GetFiles", Code.Type("string"))
                                                         // .FormatName(configuration)
                                                         .WithParameter(Code.Type("string"), "id");
            getFilesMethod.Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                          .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFilesFragment.Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                          .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null(),
                                                             Code.Null(),
                                                             Code.Local("string").Method("Join", Code.Local("Environment").Property("NewLine"),
                                                                                         Code.Local("output").Property("Files").Method("Select", Code.Lambda("x", Code.Local("x").Property("Key")))))));

            ChainedCodeFragment getFromCacheForFileFragment = configuration.Template.UseOwnCache
                                                                  ? (ChainedCodeFragment)Code.Local("cache")
                                                                  : Code.Static(Code.Type("HttpContext")).Property("Current").Property("Cache");
            MethodTemplate getFileMethod = classTemplate.AddMethod("GetFile", Code.Type("string"))
                                                        // .FormatName(configuration)
                                                        .WithParameter(Code.Type("string"), "id")
                                                        .WithParameter(Code.Type("string"), "path");
            getFileMethod.Code.AddLine(Code.If(Code.Local("id").Equals().Null(), x => x.Code.AddLine(Code.Return(Code.Null()))))
                         .AddLine(Code.Declare(Code.Type("MemoryOutput"), "output", getFromCacheForFileFragment.Index(Code.Local("id")).As(Code.Type("MemoryOutput"))))
                         .AddLine(Code.Return(Code.InlineIf(Code.Local("output").Equals().Null().Or().Not().Local("output").Property("Files").Method("ContainsKey", Code.Local("path")),
                                                            Code.Null(),
                                                            Code.Local("output").Property("Files").Index(Code.Local("path")))));
            MethodTemplate availableMethod = classTemplate.AddMethod("Available", Code.Type("bool"));
                                                          // .FormatName(configuration);
            availableMethod.Code.AddLine(Code.Return(Code.Local("true")));

            if (configuration.Template.UseAttributes)
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
        }
    }
}
