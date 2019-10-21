using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configurations;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Writers
{
    public class AngularServiceWriter : TransferWriter
    {
        public AngularServiceWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        public virtual void Write(AngularWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            Logger.Trace("Generate angular service for ASP.net controller...");
            if (!configuration.Language.IsTypeScript())
            {
                throw new InvalidOperationException($"Can not generate service for ASP.net Controller for language {configuration.Language?.Name ?? "Empty"}");
            }
            foreach (HttpServiceTransferObject controller in transferObjects.OfType<HttpServiceTransferObject>())
            {
                Dictionary<HttpServiceActionParameterTransferObject, ParameterTemplate> mapping = new Dictionary<HttpServiceActionParameterTransferObject, ParameterTemplate>();
                string controllerName = controller.Name.TrimEnd("Controller");
                ClassTemplate classTemplate = files.AddFile(configuration.Service.RelativePath, configuration.AddHeader)
                                                   .AddNamespace(string.Empty)
                                                   .AddClass(controllerName + "Service")
                                                   .FormatName(configuration.Language, configuration.FormatNames)
                                                   .WithUsing("HttpClient", "@angular/common/http")
                                                   .WithUsing("Injectable", "@angular/core")
                                                   .WithUsing("Observable", "rxjs")
                                                   .WithUsing("Subject", "rxjs")
                                                   .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate httpField = classTemplate.AddField("http", Code.Type("HttpClient")).Readonly().FormatName(configuration.Language, configuration.FormatNames);
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrl", Code.Type("string")).Public().FormatName(configuration.Language, configuration.FormatNames).Default(Code.String(string.Empty));
                classTemplate.AddConstructor().WithParameter(Code.Type("HttpClient"), "http")
                             .WithCode(Code.This().Field(httpField).Assign(Code.Local("http")).Close());
                string relativeModelPath = FileSystem.RelativeTo(configuration.Model?.RelativePath ?? ".", configuration.Service.RelativePath);
                foreach (HttpServiceActionTransferObject action in controller.Actions)
                {
                    ICodeFragment errorCode = Code.Lambda("error", Code.Local("subject").Method("error", Code.Local("error")));
                    this.MapType(controller.Language, configuration.Language, action.ReturnType);
                    TypeTemplate returnType = action.ReturnType.ToTemplate();
                    this.AddUsing(action.ReturnType, classTemplate, configuration.Language, relativeModelPath);
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", returnType))
                                                                 .FormatName(configuration.Language, configuration.FormatNames);
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        this.MapType(controller.Language, configuration.Language, parameter.Type);
                        this.AddUsing(parameter.Type, classTemplate, configuration.Language, relativeModelPath);
                        ParameterTemplate parameterTemplate = methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameter.Name).FormatName(configuration.Language, configuration.FormatNames);
                        mapping.Add(parameter, parameterTemplate);
                    }
                    TypeTemplate subjectType = Code.Generic("Subject", returnType);
                    methodTemplate.WithCode(Code.Declare(subjectType, "subject", Code.New(subjectType)));
                    string uri = ("/" + controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') + "/" + action.Route?.Replace("[action]", action.Name.ToLower())).TrimEnd('/');

                    List<HttpServiceActionParameterTransferObject> urlParameters = action.Parameters.Where(x => !x.FromBody && x.AppendName).ToList();
                    List<HttpServiceActionParameterTransferObject> urlDirectParameters = action.Parameters.Where(x => !x.FromBody && !x.AppendName).ToList();
                    uri = urlParameters.Count > 0 ? $"{uri}?{urlParameters.First().Name}=" : urlDirectParameters.Count > 0 ? $"{uri}?" : uri;
                    MultilineCodeFragment code = Code.Multiline();
                    DeclareTemplate declareTemplate = null;
                    bool hasReturnType = returnType.Name != "void";
                    if (returnType.Name == "Array")
                    {
                        TypeTemplate type = ((GenericTypeTemplate)returnType).Types[0];
                        declareTemplate = Code.Declare(returnType, "list", Code.TypeScript("[]")).Constant();
                        code.AddLine(declareTemplate)
                            .AddLine(Code.TypeScript("for (const entry of <[]>result)").StartBlock())
                            .AddLine(Code.Local(declareTemplate).Method("push", Code.New(type, Code.Local("entry"))).Close())
                            .AddLine(Code.TypeScript("").EndBlock());
                    }
                    else if (hasReturnType)
                    {
                        declareTemplate = Code.Declare(returnType, "model", Code.New(returnType, Code.Local("result"))).Constant();
                        code.AddLine(declareTemplate);
                    }
                    code.AddLine(Code.Local("subject").Method("next").WithParameter(declareTemplate.ToLocal()).Close())
                        .AddLine(Code.Local("subject").Method("complete").Close());
                    ChainedCodeFragment parameterUrl = Code.This().Field(serviceUrlField).Append(Code.String(uri));
                    bool isFirst = true;
                    foreach (HttpServiceActionParameterTransferObject parameter in urlDirectParameters)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            parameterUrl = parameterUrl.Append(Code.Local(parameter.Name));
                        }
                        else
                        {
                            parameterUrl = parameterUrl.Append(Code.String("&")).Append(Code.Local(parameter.Name));
                        }
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in urlParameters)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            parameterUrl = parameterUrl.Append(Code.Local(mapping[parameter]));
                        }
                        else
                        {
                            parameterUrl = parameterUrl.Append(Code.String($"&{parameter.Name}=")).Append(Code.Local(mapping[parameter]));
                        }
                    }

                    methodTemplate.WithCode(
                        Code.This()
                            .Field(httpField)
                            .Method(action.Type.ToString().ToLowerInvariant(), parameterUrl, action.RequireBodyParameter ? Code.Local(action.Parameters.Single(x => x.FromBody).Name) : null)
                            .Method("subscribe", Code.Lambda(hasReturnType ? "result" : null, code), errorCode).Close()
                    );
                    methodTemplate.WithCode(Code.Return(Code.Local("subject")));
                }
            }
        }
    }
}