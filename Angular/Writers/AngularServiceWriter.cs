using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Angular.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
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
            string httpClient = configuration.Service.HttpClient?.Name ?? "HttpClient";
            string httpClientImport = configuration.Service.HttpClient?.Import ?? "@angular/common/http";
            foreach (HttpServiceTransferObject controller in transferObjects.OfType<HttpServiceTransferObject>())
            {
                IMappableLanguage controllerLanguage = controller.Language as IMappableLanguage;
                IMappableLanguage configurationLanguage = configuration.Language as IMappableLanguage;
                Dictionary<HttpServiceActionParameterTransferObject, ParameterTemplate> mapping = new Dictionary<HttpServiceActionParameterTransferObject, ParameterTemplate>();
                string controllerName = controller.Name.TrimEnd("Controller");
                FileTemplate file = files.AddFile(configuration.Service.RelativePath, configuration.AddHeader, configuration.CheckOnOverwrite);
                ClassTemplate classTemplate = file.AddNamespace(string.Empty)
                                                  .AddClass(configuration.Service.Name?.Replace("{0}", controllerName) ?? controllerName + "Service")
                                                  .FormatName(configuration)
                                                  .WithUsing(httpClient, httpClientImport)
                                                  .WithUsing("Injectable", "@angular/core")
                                                  .WithUsing("Observable", "rxjs")
                                                  .WithUsing("Subject", "rxjs")
                                                  .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate httpField = classTemplate.AddField("http", Code.Type(httpClient)).Readonly().FormatName(configuration);
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrl", Code.Type("string")).Public().FormatName(configuration).Default(Code.String(string.Empty));
                classTemplate.AddConstructor().WithParameter(Code.Type(httpClient), "http")
                             .WithCode(Code.This().Field(httpField).Assign(Code.Local("http")).Close());
                string relativeModelPath = FileSystem.RelativeTo(configuration.Model?.RelativePath ?? ".", configuration.Service.RelativePath);
                bool appendConvertAnyMethod = false;
                bool appendConvertDateMethod = false;
                foreach (HttpServiceActionTransferObject action in controller.Actions)
                {
                    ICodeFragment errorCode = Code.Lambda("error", Code.Local("subject").Method("error", Code.Local("error")));
                    if (controllerLanguage != null && configurationLanguage != null)
                    {
                        this.MapType(controllerLanguage, configurationLanguage, action.ReturnType);
                    }
                    TypeTemplate returnType = action.ReturnType.ToTemplate();
                    this.AddUsing(action.ReturnType, classTemplate, configuration, relativeModelPath);
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", returnType))
                                                                 .FormatName(configuration);
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (controllerLanguage != null && configurationLanguage != null)
                        {
                            this.MapType(controllerLanguage, configurationLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, configuration, relativeModelPath);
                        ParameterTemplate parameterTemplate = methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameter.Name, parameter.IsOptional ? Code.Null() : null).FormatName(configuration);
                        mapping.Add(parameter, parameterTemplate);
                    }
                    methodTemplate.AddParameter(Code.Type("{}"), "httpOptions", Code.Null());
                    TypeTemplate subjectType = Code.Generic("Subject", returnType);
                    methodTemplate.WithCode(Code.Declare(subjectType, "subject", Code.New(subjectType)));
                    string uri = ("/" + (controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') ?? controllerName) + "/" + action.Route?.Replace("[action]", action.Name.ToLower())).TrimEnd('/');

                    List<HttpServiceActionParameterTransferObject> inlineParameters = action.Parameters.Where(x => !x.FromBody && x.Inline).OrderBy(x => x.InlineIndex).ToList();
                    List<HttpServiceActionParameterTransferObject> urlParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && x.AppendName).ToList();
                    List<HttpServiceActionParameterTransferObject> urlDirectParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && !x.AppendName).ToList();
                    uri = urlParameters.Count > 0 ? $"{uri}?{urlParameters.First().Name}=" : urlDirectParameters.Count > 0 ? $"{uri}?" : uri;
                    MultilineCodeFragment code = Code.Multiline();
                    DeclareTemplate declareTemplate = null;
                    bool hasReturnType = returnType.Name != "void";
                    bool isPrimitive = this.IsPrimitive(returnType);
                    if (returnType.Name == "Array")
                    {
                        TypeTemplate type = ((GenericTypeTemplate)returnType).Types[0];
                        ICodeFragment createModelCode = isPrimitive
                                                            ? (ICodeFragment)Code.Cast(type, Code.Local("entry"))
                                                            : Code.InlineIf(Code.Local("entry").Equals().ForceNull().Or().Local("entry").Equals().Undefined(),
                                                                            Code.Undefined(),
                                                                            Code.New(type, Code.Local("entry"))
                                                            );
                        declareTemplate = Code.Declare(returnType, "list", Code.TypeScript("[]")).Constant();
                        code.AddLine(declareTemplate)
                            .AddLine(Code.TypeScript("for (const entry of result) {").BreakLine() /*.StartBlock()*/)
                            .AddLine(Code.Local(declareTemplate).Method("push", createModelCode).Close())
                            //.AddLine(Code.TypeScript("").EndBlock());
                            .AddLine(Code.TypeScript("}").BreakLine());
                    }
                    else if (hasReturnType)
                    {
                        ICodeFragment createModelCode = isPrimitive
                                                            ? (ICodeFragment)Code.Cast(returnType, Code.Local("result"))
                                                            : Code.InlineIf(Code.Local("result").Equals().ForceNull().Or().Local("result").Equals().Undefined(),
                                                                            Code.Undefined(),
                                                                            Code.New(returnType, Code.Local("result"))
                                                            );
                        declareTemplate = Code.Declare(returnType, "model", createModelCode).Constant();
                        code.AddLine(declareTemplate);
                    }
                    code.AddLine(Code.Local("subject").Method("next").WithParameter(declareTemplate.ToLocal()).Close())
                        .AddLine(Code.Local("subject").Method("complete").Close());
                    ChainedCodeFragment parameterUrl = Code.This().Field(serviceUrlField);
                    if (inlineParameters.Count == 0)
                    {
                        parameterUrl = parameterUrl.Append(Code.String(uri));
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in inlineParameters)
                    {
                        string[] chunks = uri.Split(new[] { $"{{{parameter.Name}}}" }, StringSplitOptions.RemoveEmptyEntries);
                        parameterUrl = parameterUrl.Append(Code.String(chunks[0])).Append(Code.Local(parameter.Name));
                        uri = chunks.Length == 1 ? string.Empty : chunks[1];
                    }
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
                        string name = mapping[parameter].Name;
                        if (!isFirst)
                        {
                            parameterUrl = parameterUrl.Append(Code.String($"&{parameter.Name}="));
                        }
                        isFirst = false;
                        if (parameter.Type.IgnoreNullable().Name == "Date")
                        {
                            appendConvertDateMethod = true;
                            parameterUrl = parameterUrl.Append(Code.This().Method("convertDate", Code.Local(name)));
                        }
                        else
                        {
                            appendConvertAnyMethod = true;
                            parameterUrl = parameterUrl.Append(Code.This().Method("convertAny", Code.Local(name)));
                        }
                    }

                    methodTemplate.WithCode(
                        Code.This()
                            .Field(httpField)
                            .GenericMethod(action.Type.ToString().ToLowerInvariant(),
                                           returnType,
                                           parameterUrl,
                                           action.RequireBodyParameter ? Code.Local(action.Parameters.Single(x => x.FromBody).Name) : null,
                                           Code.Local("httpOptions")
                            )
                            .Method("subscribe", Code.Lambda(hasReturnType ? "result" : null, code), errorCode).Close()
                    );
                    methodTemplate.WithCode(Code.Return(Code.Local("subject")));
                }

                if (appendConvertAnyMethod)
                {
                    classTemplate.AddMethod("convertAny", Code.Type("string"))
                                 .WithParameter(Code.Type("any"), "value")
                                 .WithCode(Code.Return(Code.InlineIf(Code.Local("value").Equals().ForceNull().Or().Local("value").Equals().Undefined(),
                                                                     Code.String(string.Empty),
                                                                     Code.Local("value").Method("toString")
                                                       )
                                           ));
                }
                if (appendConvertDateMethod)
                {
                    classTemplate.AddMethod("convertDate", Code.Type("string"))
                                 .WithParameter(Code.Type("Date"), "date")
                                 .WithCode(Code.Return(Code.InlineIf(Code.Local("date").Equals().ForceNull().Or().Local("date").Equals().Undefined(),
                                                                     Code.String(string.Empty),
                                                                     Code.InlineIf(Code.TypeScript($"typeof(date) === \"string\""),
                                                                                   Code.Local("date"),
                                                                                   Code.Local("date").Method("toISOString")
                                                                     )
                                                       )
                                           ));
                }
            }
        }

        private bool IsPrimitive(TypeTemplate type)
        {
            return type is GenericTypeTemplate genericType
                       ? this.IsPrimitive(genericType.Types.First())
                       : type.Name == "string" || type.Name == "number" || type.Name == "boolean";
        }
    }
}