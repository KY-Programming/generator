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
using KY.Generator.TypeScript;
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
                        this.MapType(controller.Language, configuration.Language, parameter.Type);
                        this.AddUsing(parameter.Type, classTemplate, configuration.Language, relativeModelPath);
                        methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameter.Name);
                    }
                    TypeTemplate subjectType = Code.Generic("Subject", returnType);
                    methodTemplate.WithCode(Code.Declare(subjectType, "subject", Code.New(subjectType)));
                    string uri = "/" + controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') + "/" + action.Route?.Replace("[action]", action.Name.ToLower());
                    if (action.Type == AspDotNetControllerActionType.Get)
                    {
                        uri = action.Parameters.Count > 0 ? $"{uri}?{action.Parameters[0].Name}=" : uri;
                        MultilineCodeFragment code = Code.Multiline();
                        bool isArray = returnType.Name == "Array";
                        if (isArray)
                        {
                            code.AddLine(Code.TypeScript("const list: Resource[] = []").Close())
                                .AddLine(Code.TypeScript("for (const entry of <[]>result)").StartBlock())
                                .AddLine(Code.TypeScript("list.push(new Resource(entry))").Close())
                                .AddLine(Code.TypeScript("").EndBlock());
                        }
                        else
                        {
                            code.AddLine(Code.Declare(returnType, "model", Code.New(returnType, Code.Local("result"))));
                        }
                        code.AddLine(Code.Local("subject").Method("next").WithParameter(Code.Local("model")).Close())
                            .AddLine(Code.Local("subject").Method("complete").Close());
                        ChainedCodeFragment parameterUrl = Code.This().Field(serviceUrlField).Append(Code.String(uri));
                        bool isFirst = true;
                        foreach (AspDotNetControllerActionParameter parameter in action.Parameters)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                parameterUrl = parameterUrl.Append(Code.Local(parameter.Name));
                            }
                            else
                            {
                                parameterUrl = parameterUrl.Append(Code.String($"&{parameter.Name}=")).Append(Code.Local(parameter.Name));
                            }
                        }
                        methodTemplate.WithCode(Code.This().Field(httpField).Method("get", parameterUrl).Method("subscribe", Code.Lambda("result", code)).Close());
                    }
                    else// if (action.Type == AspDotNetControllerActionType.Post)
                    {
                        AspDotNetControllerActionParameter bodyParameter = action.Parameters.Single();
                        MultilineCodeFragment code = Code.Multiline()
                                                         //.AddLine(Code.Declare(returnType, "model", Code.New(returnType, Code.Local("result"))))
                                                         .AddLine(Code.Local("subject").Method("next") /*.WithParameter(Code.Local("model"))*/.Close())
                                                         .AddLine(Code.Local("subject").Method("complete").Close());
                        methodTemplate.WithCode(Code.This().Field(httpField).Method(action.Type.ToString().ToLowerInvariant(),  Code.This().Field(serviceUrlField).Append(Code.String(uri)), Code.Local(bodyParameter.Name)).Method("subscribe", Code.Lambda("result", code)).Close());
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