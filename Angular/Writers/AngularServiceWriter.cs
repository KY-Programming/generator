using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers
{
    internal class AngularServiceWriter : TransferWriter
    {
        private readonly ModelWriter modelWriter;

        public AngularServiceWriter(TypeScriptModelWriter modelWriter, ITypeMapping typeMapping)
            : base(typeMapping)
        {
            this.modelWriter = modelWriter;
        }

        public void Write(AngularConfiguration configuration, List<ITransferObject> transferObjects, IOutput output)
        {
            Logger.Trace("Generate angular service for ASP.net controller...");
            if (!configuration.Language.IsTypeScript())
            {
                throw new InvalidOperationException($"Can not generate service for ASP.net Controller for language {configuration.Language?.Name ?? "Empty"}");
            }
            List<FileTemplate> files = new List<FileTemplate>();
            foreach (AspDotNetController controller in transferObjects.OfType<AspDotNetController>())
            {
                string controllerName = controller.Name.TrimEnd("Controller");
                ClassTemplate classTemplate = files.AddFile(configuration.Service.RelativePath)
                                                   .AddNamespace(string.Empty)
                                                   .AddClass(controllerName + "Service")
                                                   .WithUsing("HttpClient", "@angular/common/http")
                                                   .WithUsing("Injectable", "@angular/core")
                                                   .WithUsing("Observable", "rxjs")
                                                   .WithUsing("Subject", "rxjs")
                                                   .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate httpField = classTemplate.AddField("http", Code.Type("HttpClient")).Readonly().FormatName(configuration.Language, configuration.FormatNames);
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrl", Code.Type("string")).Public().FormatName(configuration.Language, configuration.FormatNames).Default(Code.String(string.Empty));
                classTemplate.AddConstructor().WithParameter(Code.Type("HttpClient"), "http")
                             .WithCode(Code.This().Field(httpField).Assign(Code.Local("http")).Close());
                string relativeModelPath = FileSystem.RelativeTo(configuration.Service.ModelPath, configuration.Service.RelativePath);
                foreach (AspDotNetControllerAction action in controller.Actions)
                {
                    this.MapType(controller.Language, configuration.Language, action.ReturnType);
                    TypeTemplate returnType = action.ReturnType.ToTemplate();
                    this.AddUsing(action.ReturnType, classTemplate, configuration.Language, relativeModelPath);
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", returnType))
                                                                 .FormatName(configuration.Language, configuration.FormatNames);
                    foreach (AspDotNetControllerActionParameter parameter in action.Parameters)
                    {
                        methodTemplate.AddParameter(Code.Type(parameter.Type), parameter.Name);
                    }
                    TypeTemplate subjectType = Code.Generic("Subject", returnType);
                    methodTemplate.WithCode(Code.Declare(subjectType, "subject", Code.New(subjectType)));
                    string uri = controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') + "/" + action.Route?.Replace("[action]", action.Name.ToLower());
                    if (action.Type == AspDotNetControllerActionType.Get)
                    {
                        MultilineCodeFragment code = Code.Multiline()
                                                         .AddLine(Code.Declare(returnType, "model", Code.New(returnType, Code.Local("result"))))
                                                         .AddLine(Code.Local("subject").Method("next").WithParameter(Code.Local("model")).Close())
                                                         .AddLine(Code.Local("subject").Method("complete").Close());
                        methodTemplate.WithCode(Code.This().Field(httpField).Method("get", Code.String(uri)).Method("subscribe", Code.Lambda("result", code)));
                    }
                    else if (action.Type == AspDotNetControllerActionType.Post)
                    {
                        AspDotNetControllerActionParameter bodyParameter = action.Parameters.Single();
                        MultilineCodeFragment code = Code.Multiline()
                                                         //.AddLine(Code.Declare(returnType, "model", Code.New(returnType, Code.Local("result"))))
                                                         .AddLine(Code.Local("subject").Method("next") /*.WithParameter(Code.Local("model"))*/.Close())
                                                         .AddLine(Code.Local("subject").Method("complete").Close());
                        methodTemplate.WithCode(Code.This().Field(httpField).Method("post", Code.String(uri), Code.Local(bodyParameter.Name)).Method("subscribe", Code.Lambda("result", code)));
                    }
                    methodTemplate.WithCode(Code.Return(Code.Local("subject")));
                }
            }

            ModelWriteConfiguration modelWriteConfiguration = new ModelWriteConfiguration();
            modelWriteConfiguration.CopyBaseFrom(configuration);
            modelWriteConfiguration.Name = configuration.Service.Name;
            modelWriteConfiguration.Namespace = configuration.Service.Namespace;
            modelWriteConfiguration.RelativePath = configuration.Service.ModelPath ?? configuration.Service.RelativePath;
            modelWriteConfiguration.SkipNamespace = configuration.Service.SkipNamespace;
            modelWriteConfiguration.PropertiesToFields = configuration.Service.PropertiesToFields;
            modelWriteConfiguration.FieldsToProperties = configuration.Service.FieldsToProperties;
            modelWriteConfiguration.FormatNames = configuration.FormatNames;
            this.modelWriter.Write(modelWriteConfiguration, transferObjects).ForEach(files.Add);

            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}