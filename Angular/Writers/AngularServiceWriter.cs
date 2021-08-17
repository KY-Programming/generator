﻿using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.Angular.Writers
{
    public class AngularServiceWriter : TransferWriter
    {
        private const string apiVersionKey = "{version:apiVersion}";

        public AngularServiceWriter(ITypeMapping typeMapping, Options options)
            : base(typeMapping, options)
        { }

        public virtual void Write(List<ITransferObject> transferObjects, AngularWriteConfiguration configuration, IOutput output)
        {
            List<FileTemplate> files = new();
            this.Write(transferObjects, configuration, files);
            files.ForEach(file => this.Options.Current.Language.Write(file, output));
        }

        public virtual void Write(List<ITransferObject> transferObjects, AngularWriteConfiguration configuration, List<FileTemplate> files)
        {
            Logger.Trace("Generate angular service for ASP.NET controller...");
            if (!this.Options.Current.Language.IsTypeScript())
            {
                throw new InvalidOperationException($"Can not generate service for ASP.NET controller for language {this.Options.Current.Language?.Name ?? "Empty"}");
            }
            if (configuration.Model?.RelativePath == null && configuration.Service.RelativePath?.Count(x => x == '/' || x == '\\') > 1)
            {
                Logger.Warning("No model path found for Angular Service command. This may lead to wrong model imports!");
            }
            string httpClient = configuration.Service.HttpClient?.Name ?? "HttpClient";
            string httpClientImport = configuration.Service.HttpClient?.Import ?? "@angular/common/http";
            Dictionary<HttpServiceActionTypeTransferObject, string> actionTypeMapping = new()
                                                                                        {
                                                                                            { HttpServiceActionTypeTransferObject.Get, configuration.Service?.HttpClient?.Get ?? "get" },
                                                                                            { HttpServiceActionTypeTransferObject.Post, configuration.Service?.HttpClient?.Post ?? "post" },
                                                                                            { HttpServiceActionTypeTransferObject.Put, configuration.Service?.HttpClient?.Put ?? "put" },
                                                                                            { HttpServiceActionTypeTransferObject.Patch, configuration.Service?.HttpClient?.Patch ?? "patch" },
                                                                                            { HttpServiceActionTypeTransferObject.Delete, configuration.Service?.HttpClient?.Delete ?? "delete" }
                                                                                        };
            Dictionary<HttpServiceActionTypeTransferObject, AngularHttpClientMethodOptions> actionTypeOptions = new()
                                                                                                                {
                                                                                                                    { HttpServiceActionTypeTransferObject.Get, configuration.Service?.HttpClient?.HasGetOptions },
                                                                                                                    { HttpServiceActionTypeTransferObject.Post, configuration.Service?.HttpClient?.HasPostOptions },
                                                                                                                    { HttpServiceActionTypeTransferObject.Put, configuration.Service?.HttpClient?.HasPutOptions },
                                                                                                                    { HttpServiceActionTypeTransferObject.Patch, configuration.Service?.HttpClient?.HasPatchOptions },
                                                                                                                    { HttpServiceActionTypeTransferObject.Delete, configuration.Service?.HttpClient?.HasDeleteOptions }
                                                                                                                };
            foreach (HttpServiceTransferObject controller in transferObjects.OfType<HttpServiceTransferObject>())
            {
                IOptions controllerOptions = this.Options.Get(controller);
                IMappableLanguage controllerLanguage = controller.Language as IMappableLanguage;
                IMappableLanguage outputLanguage = controllerOptions.Language as IMappableLanguage;
                Dictionary<HttpServiceActionParameterTransferObject, ParameterTemplate> mapping = new();
                string controllerName = controller.Name.TrimEnd("Controller");
                FileTemplate file = files.AddFile(configuration.Service.RelativePath, controllerOptions.AddHeader, controllerOptions.OutputId)
                                         .WithType("service");
                ClassTemplate classTemplate = file.AddNamespace(string.Empty)
                                                  .AddClass(configuration.Service.Name?.Replace("{0}", controllerName) ?? controllerName + "Service")
                                                  .FormatName(controllerOptions, true)
                                                  .WithUsing(httpClient, httpClientImport)
                                                  .WithUsing("Injectable", "@angular/core")
                                                  .WithUsing("Observable", "rxjs")
                                                  .WithUsing("Subject", "rxjs")
                                                  .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate httpField = classTemplate.AddField("http", Code.Type(httpClient)).Readonly().FormatName(controllerOptions);
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrlValue", Code.Type("string")).FormatName(controllerOptions).Default(Code.String(string.Empty));
                PropertyTemplate serviceUrlProperty = classTemplate.AddProperty("serviceUrl", Code.Type("string"))
                                                                   .WithGetter(Code.Return(Code.This().Field(serviceUrlField)))
                                                                   .WithSetter(Code.This().Field(serviceUrlField).Assign(Code.Local("value").Method("replace", Code.TypeScript(@"/\/+$/"), Code.String(""))).Close());
                classTemplate.AddConstructor().WithParameter(Code.Type(httpClient), "http")
                             .WithCode(Code.This().Field(httpField).Assign(Code.Local("http")).Close());
                string relativeModelPath = FileSystem.RelativeTo(configuration.Model?.RelativePath ?? ".", configuration.Service.RelativePath);
                relativeModelPath = string.IsNullOrEmpty(relativeModelPath) ? "." : relativeModelPath;
                bool appendConvertAnyMethod = false;
                bool appendConvertFromDateMethod = false;
                bool appendConvertToDateMethod = false;
                foreach (HttpServiceActionTransferObject action in controller.Actions)
                {
                    string subjectName = action.Parameters.Any(x => x.Name == "subject") ? "rxjsSubject" : "subject";
                    bool isEnumerable = action.ReturnType.IsEnumerable();
                    bool isGuidReturnType = action.ReturnType.Name.Equals(nameof(Guid), StringComparison.CurrentCultureIgnoreCase);
                    bool isDateReturnType = action.ReturnType.Name.Equals(nameof(DateTime), StringComparison.CurrentCultureIgnoreCase);
                    bool isDateArrayReturnType = isEnumerable && action.ReturnType.Generics.Count == 1 && action.ReturnType.Generics.First().Type.Name.Equals(nameof(DateTime), StringComparison.CurrentCultureIgnoreCase);
                    bool isStringReturnType = action.ReturnType.Name.Equals(nameof(String), StringComparison.CurrentCultureIgnoreCase);
                    ICodeFragment errorCode = Code.Lambda("error", Code.Local(subjectName).Method("error", Code.Local("error")));
                    if (controllerLanguage != null && outputLanguage != null)
                    {
                        this.MapType(controllerLanguage, outputLanguage, action.ReturnType);
                    }
                    TypeTemplate returnType = action.ReturnType.ToTemplate();
                    TypeTransferObject returnModelType = isEnumerable ? action.ReturnType.Generics.First().Type : action.ReturnType;
                    ModelTransferObject returnModel = returnModelType as ModelTransferObject ?? transferObjects.OfType<ModelTransferObject>().FirstOrDefault(x => x.Equals(returnModelType));
                    this.AddUsing(action.ReturnType, classTemplate, controllerOptions, relativeModelPath);
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", returnType))
                                                                 .FormatName(controllerOptions);
                    TypeTemplate subjectType = Code.Generic("Subject", returnType);
                    methodTemplate.WithCode(Code.Declare(subjectType, subjectName, Code.New(subjectType)));
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (controllerLanguage != null && outputLanguage != null)
                        {
                            this.MapType(controllerLanguage, outputLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, controllerOptions, relativeModelPath);
                        string parameterName = this.GetAllowedName(controllerOptions.Language, parameter.Name);
                        ParameterTemplate parameterTemplate = methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameterName).FormatName(controllerOptions);
                        if (parameter.IsOptional)
                        {
                            parameterTemplate.Optional();
                        }
                        mapping.Add(parameter, parameterTemplate);
                        if (action.Type == HttpServiceActionTypeTransferObject.Get && parameter.Type.Name == "Array")
                        {
                            methodTemplate.WithCode(Code.Declare(
                                                        Code.Type("string"),
                                                        $"{parameterTemplate.Name}Join",
                                                        Code.Local(parameterTemplate).Method("map", Code.Lambda(new[] { "x", "index" },
                                                                                                                Code.InlineIf(Code.Local("index"),
                                                                                                                              Code.String($"{parameter.Name}=").Append(Code.This().Method("convertAny", Code.Local("x"))),
                                                                                                                              Code.This().Method("convertAny", Code.Local("x")))
                                                                                             )).Method("join", Code.String("&"))));
                        }
                    }
                    if (actionTypeOptions[action.Type]?.HasHttpOptions ?? true)
                    {
                        methodTemplate.AddParameter(Code.Type("{}"), "httpOptions").Optional();
                        if (isStringReturnType)
                        {
                            methodTemplate.WithCode(Code.TypeScript("httpOptions = { responseType: 'text', ...httpOptions}").Close());
                        }
                    }
                    if (isDateReturnType && isDateArrayReturnType)
                    {
                        appendConvertToDateMethod = true;
                    }
                    string uri = ("/" + (controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') ?? controllerName.ToLower()) + "/" + action.Route?.Replace("[action]", action.Name.ToLower())).TrimEnd('/');

                    List<HttpServiceActionParameterTransferObject> inlineParameters = action.Parameters.Where(x => !x.FromBody && x.Inline).OrderBy(x => x.InlineIndex).ToList();
                    List<HttpServiceActionParameterTransferObject> urlParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && x.AppendName).ToList();
                    List<HttpServiceActionParameterTransferObject> urlDirectParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && !x.AppendName).ToList();
                    MultilineCodeFragment code = Code.Multiline();
                    bool hasReturnType = returnType.Name != "void" && returnType.Name != "Task";
                    ExecuteMethodTemplate nextMethod = Code.Local(subjectName).Method("next");
                    if (hasReturnType)
                    {
                        LocalVariableTemplate localCode = Code.Local("result");
                        ICodeFragment nextCode = localCode;
                        if (isGuidReturnType)
                        {
                            nextCode = localCode.Method("replace", Code.TypeScript("/(^\"|\"$)/g"), Code.String(string.Empty));
                        }
                        if (isDateReturnType)
                        {
                            nextCode = Code.This().Method("convertToDate", nextCode);
                        }
                        if (isDateArrayReturnType)
                        {
                            nextCode = localCode.Method("map", Code.Lambda("entry", Code.This().Method("convertToDate", Code.Local("entry"))));
                        }
                        nextMethod.WithParameter(nextCode);
                        appendConvertToDateMethod = this.WriteDateFixes(returnModel, isEnumerable, code, new List<string> { "result" }, new List<TypeTransferObject> { returnModel }, transferObjects) || appendConvertToDateMethod;
                    }
                    if (action.FixCasingWithMapping)
                    {
                        IEnumerable<MemberTransferObject> members = (returnModel?.Fields ?? new List<FieldTransferObject>()).Concat<MemberTransferObject>(returnModel?.Properties ?? new List<PropertyTransferObject>());
                        if (isEnumerable)
                        {
                            MultilineCodeFragment innerCode = new();
                            foreach (MemberTransferObject member in members)
                            {
                                string formattedName = member is PropertyTransferObject ? Formatter.FormatProperty(member.Name, controllerOptions) : Formatter.FormatField(member.Name, controllerOptions);
                                if (formattedName != member.Name)
                                {
                                    innerCode.AddLine(Code.Local("entry").Field(formattedName).Assign(Code.Local("entry").Field(formattedName).Or().Local("entry").Index(Code.String(member.Name))).Close())
                                             .AddLine(Code.TypeScript($"delete entry['{member.Name}']").Close());
                                }
                            }
                            code.AddLine(Code.If(Code.Local("result")).WithCode(Code.Local("result").Method("forEach", Code.Lambda("entry", innerCode))));
                        }
                        else
                        {
                            foreach (MemberTransferObject member in members)
                            {
                                string formattedName = member is PropertyTransferObject ? Formatter.FormatProperty(member.Name, controllerOptions) : Formatter.FormatField(member.Name, controllerOptions);
                                if (formattedName != member.Name)
                                {
                                    code.AddLine(Code.Local("result").Field(formattedName).Assign(Code.Local("result").Field(formattedName).Or().Local("result").Index(Code.String(member.Name))).Close())
                                        .AddLine(Code.TypeScript($"delete result['{member.Name}']").Close());
                                }
                            }
                        }
                    }
                    code.AddLine(nextMethod.Close())
                        .AddLine(Code.Local(subjectName).Method("complete").Close());
                    ChainedCodeFragment parameterUrl = Code.This().Property(serviceUrlProperty);
                    string actionVersion = action.Version ?? controller.Version;
                    bool isUrlApiVersion = false;
                    if (uri?.Contains(apiVersionKey) ?? false)
                    {
                        isUrlApiVersion = true;
                        uri = uri.Replace(apiVersionKey, actionVersion);
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in inlineParameters)
                    {
                        string[] chunks = uri.Split(new[] { $"{{{parameter.Name}}}" }, StringSplitOptions.RemoveEmptyEntries);
                        parameterUrl = parameterUrl.Append(Code.String(chunks[0])).Append(Code.Local(mapping[parameter]));
                        uri = chunks.Length == 1 ? string.Empty : chunks[1];
                    }
                    if (!string.IsNullOrEmpty(uri))
                    {
                        parameterUrl = parameterUrl.Append(Code.String(uri));
                    }
                    bool isFirst = true;
                    if (actionVersion != null && !isUrlApiVersion)
                    {
                        isFirst = false;
                        parameterUrl = parameterUrl.Append(Code.String($"?api-version={actionVersion}"));
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in urlDirectParameters)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            parameterUrl = parameterUrl.Append(Code.String("?")).Append(Code.Local(parameter.Name));
                        }
                        else
                        {
                            parameterUrl = parameterUrl.Append(Code.String("&")).Append(Code.Local(parameter.Name));
                        }
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in urlParameters)
                    {
                        parameterUrl = parameterUrl.Append(isFirst ? Code.String($"?{parameter.Name}=") : Code.String($"&{parameter.Name}="));
                        if (parameter.FromQuery && parameter.Type.Name == "Array")
                        {
                            appendConvertAnyMethod = true;
                            parameterUrl = parameterUrl.Append(Code.This().Method("convertAny", Code.Local(mapping[parameter].Name + "Join")));
                        }
                        else
                        {
                            if (parameter.Type.IgnoreNullable().Name == "Date")
                            {
                                appendConvertFromDateMethod = true;
                                parameterUrl = parameterUrl.Append(Code.This().Method("convertFromDate", Code.Local(mapping[parameter])));
                            }
                            else
                            {
                                appendConvertAnyMethod = true;
                                parameterUrl = parameterUrl.Append(Code.This().Method("convertAny", Code.Local(mapping[parameter])));
                            }
                        }
                        isFirst = false;
                    }
                    ChainedCodeFragment executeAction = Code.This().Field(httpField);
                    List<ICodeFragment> parameters = new() { parameterUrl };
                    if (action.Parameters.Any(x => x.FromBody))
                    {
                        parameters.Add(Code.Local(action.Parameters.Single(x => x.FromBody).Name));
                    }
                    if (actionTypeOptions[action.Type]?.HasHttpOptions ?? false)
                    {
                        parameters.Add(Code.Local("httpOptions"));
                    }
                    if (actionTypeOptions[action.Type]?.NotGeneric ?? false)
                    {
                        executeAction = executeAction.Method(actionTypeMapping[action.Type], parameters);
                    }
                    else if (actionTypeOptions[action.Type]?.ParameterGeneric ?? false)
                    {
                        executeAction = executeAction.GenericMethod(actionTypeMapping[action.Type], Code.Type("unknown"), parameters.ToArray());
                    }
                    else
                    {
                        executeAction = executeAction.GenericMethod(actionTypeMapping[action.Type], returnType, parameters.ToArray());
                    }
                    LambdaTemplate lambda;
                    if (actionTypeOptions[action.Type]?.ReturnsAny ?? false)
                    {
                        lambda = Code.Lambda(hasReturnType ? new ParameterTemplate(returnType, "result") : null, code);
                    }
                    else
                    {
                        lambda = Code.Lambda(hasReturnType ? "result" : null, code);
                    }
                    methodTemplate.WithCode(executeAction.Method("subscribe", lambda, errorCode).Close())
                                  .WithCode(Code.Return(Code.Local(subjectName)));
                }

                if (appendConvertAnyMethod)
                {
                    this.AppendConvertAnyMethod(classTemplate);
                }
                if (appendConvertFromDateMethod)
                {
                    this.AppendConvertFromDateMethod(classTemplate);
                }
                if (appendConvertToDateMethod)
                {
                    this.AppendConvertToDateMethod(classTemplate);
                }
            }
            List<SignalRHubTransferObject> hubs = transferObjects.OfType<SignalRHubTransferObject>().ToList();
            FileTemplate connectionStatusFileTemplate = null;
            EnumTemplate connectionStatusEnum = null;
            if (hubs.Count > 0)
            {
                IOptions anyOptions = this.Options.Get(hubs.First());
                connectionStatusFileTemplate = files.AddFile(configuration.Model.RelativePath, anyOptions.AddHeader, anyOptions.OutputId);
                connectionStatusEnum = connectionStatusFileTemplate
                                       .AddNamespace(string.Empty)
                                       .AddEnum("ConnectionStatus")
                                       .FormatName(this.Options.Get(hubs.First()))
                                       .AddValue("connecting")
                                       .AddValue("connected")
                                       .AddValue("sleeping")
                                       .AddValue("disconnected");
            }
            foreach (SignalRHubTransferObject hub in hubs)
            {
                IOptions hubOptions = this.Options.Get(hub);
                string relativeModelPath = FileSystem.RelativeTo(configuration.Model?.RelativePath ?? ".", configuration.Service.RelativePath);
                IMappableLanguage hubLanguage = hub.Language as IMappableLanguage;
                IMappableLanguage outputLanguage = hubOptions.Language as IMappableLanguage;
                FileTemplate file = files.AddFile(configuration.Service.RelativePath, hubOptions.AddHeader, hubOptions.OutputId)
                                         .WithType("service");
                NamespaceTemplate namespaceTemplate = file.AddNamespace(string.Empty);
                ClassTemplate classTemplate = namespaceTemplate
                                              .AddClass(configuration.Service.Name?.Replace("{0}", hub.Name) ?? hub.Name + "Service")
                                              .FormatName(hubOptions, true)
                                              .WithUsing("Injectable", "@angular/core")
                                              .WithUsing("Subject", "rxjs")
                                              .WithUsing("Observable", "rxjs")
                                              .WithUsing("ReplaySubject", "rxjs")
                                              .WithUsing("filter", "rxjs/operators")
                                              .WithUsing("map", "rxjs/operators")
                                              .WithUsing("mergeMap", "rxjs/operators")
                                              .WithUsing("take", "rxjs/operators")
                                              .WithUsing("HubConnectionBuilder", "@microsoft/signalr")
                                              .WithUsing("HubConnection", "@microsoft/signalr")
                                              .WithUsing("IHttpConnectionOptions", "@microsoft/signalr")
                                              .WithUsing("LogLevel", "@microsoft/signalr")
                                              .WithUsing(connectionStatusEnum.Name, FileSystem.Combine(relativeModelPath, Formatter.FormatFile(connectionStatusFileTemplate.Name, hubOptions, true)).Replace("\\", "/"))
                                              .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate isClosedField = classTemplate.AddField("isClosed", Code.Type("boolean"));
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrl", Code.Type("string")).Public().FormatName(hubOptions).Default(Code.String(string.Empty));
                FieldTemplate optionsField = classTemplate.AddField("options", Code.Type("IHttpConnectionOptions")).Public().FormatName(hubOptions);
                FieldTemplate logLevelField = classTemplate.AddField("logLevel", Code.Type("LogLevel")).Public().FormatName(hubOptions).Default(Code.Static(Code.Type("LogLevel")).Field("Error"));
                FieldTemplate connectionField = classTemplate.AddField("connection", Code.Generic("ReplaySubject", Code.Type("HubConnection")));
                FieldTemplate timeoutsField = null;
                if (configuration.Service.Timeouts?.Count > 0)
                {
                    timeoutsField = classTemplate.AddField("timeouts", Code.Generic("Array", Code.Type("number"))).Readonly()
                                                 .Default(Code.TypeScript($"[{string.Join(", ", configuration.Service.Timeouts)}]"));
                }
                FieldTemplate statusSubjectField = classTemplate.AddField("statusSubject", Code.Generic("ReplaySubject", connectionStatusEnum.ToType())).Readonly()
                                                                .Default(Code.New(Code.Generic("ReplaySubject", connectionStatusEnum.ToType()), Code.Number(1)));
                classTemplate.AddField("status$", Code.Generic("Observable", connectionStatusEnum.ToType())).FormatName(hubOptions).Readonly().Public()
                             .Default(Code.This().Local(statusSubjectField).Method("asObservable"));
                MultilineCodeFragment createConnectionCode = Code.Multiline();
                MultilineCodeFragment errorCode = Code.Multiline();
                if (timeoutsField != null)
                {
                    errorCode.AddLine(Code.If(Code.This().Field(isClosedField)).WithCode(Code.Return()))
                             .AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Field("sleeping")).Close())
                             .AddLine(Code.Declare(Code.Type("number"), "timeout", Code.This().Field(timeoutsField).Index(Code.Local("trial"))))
                             .AddLine(Code.Local("trial++").Close());
                    if (configuration.Service.EndlessTries)
                    {
                        errorCode.AddLine(Code.Local("timeout").Assign(Code.Local("timeout").Or().This().Field(timeoutsField).Index(Code.This().Field(timeoutsField).Field("length").Subtract().Number(1)).Or().Number(0)).Close());
                    }
                    else
                    {
                        errorCode.AddLine(Code.If(Code.Local("timeout").Equals().Undefined())
                                              .WithCode(Code.This().Method("disconnect").Close())
                                              .WithCode(Code.Local("subject").Method("error", Code.Local("error")).Close()).WithCode(Code.Return()));
                    }
                    errorCode.AddLine(Code.Method("setTimeout",
                                                  Code.Lambda(Code.Multiline()
                                                                  .AddLine(Code.If(Code.This().Field(isClosedField)).WithCode(Code.Return()))
                                                                  .AddLine(Code.Method("startConnection").Method("subscribe",
                                                                                                                 Code.Lambda(Code.Multiline()
                                                                                                                                 .AddLine(Code.Local("subject").Method("next").Close())
                                                                                                                                 .AddLine(Code.Local("subject").Method("complete").Close())),
                                                                                                                 Code.Lambda("innerError", Code.Local("subject").Method("error", Code.Local("innerError")))
                                                                           ))),
                                                  Code.Local("timeout")
                                      ).Close());
                }
                else
                {
                    errorCode.AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Field("disconnected")).Close())
                             .AddLine(Code.Local("subject").Method("error", Code.Local("error")).Close());
                }

                MethodTemplate connectMethod = classTemplate.AddMethod("Connect", Code.Generic("Observable", Code.Void())).FormatName(hubOptions)
                                                            .WithComment("Connects to the hub via given serviceUrl.\nAutomatically reconnects on connection loss. \nIf timeout is configured, goes to sleeping state and reconnects after the timeout")
                                                            .WithCode(Code.If(Code.Not().This().Local(serviceUrlField))
                                                                          .WithCode(Code.Throw(Code.Type("Error"), Code.String("serviceUrl can not be empty. Set it via service.serviceUrl."))))
                                                            .WithCode(Code.If(Code.This().Field(connectionField).And().Not().This().Field(isClosedField))
                                                                          .WithCode(Code.Return(Code.This().Local("status$").Method("pipe",
                                                                                                                                    Code.Method("filter", Code.Lambda("status", Code.Local("status").Equals().Local("ConnectionStatus").Field("connected"))),
                                                                                                                                    Code.Method("take", Code.Number(1)),
                                                                                                                                    Code.Method("map", Code.Lambda(Code.AnonymousObject()))
                                                                                                ))))
                                                            .WithCode(Code.This().Field(isClosedField).Assign(Code.Boolean(false)).Close())
                                                            .WithCode(Code.This().Field(connectionField).Assign(Code.InlineIf(Code.This().Field(connectionField), Code.This().Field(connectionField), Code.New(connectionField.Type, Code.Number(1)))).Close())
                                                            .WithCode(Code.Declare(Code.Type("HubConnection"), "hubConnection", Code.New(Code.Type("HubConnectionBuilder"))
                                                                                                                                    .Method("withUrl", Code.This().Local(serviceUrlField), Code.This().Local(optionsField))
                                                                                                                                    .Method("configureLogging", Code.This().Field(logLevelField))
                                                                                                                                    .Method("build")))
                                                            .WithCode(Code.Declare(Code.Type("() => Observable<void>"), "startConnection", Code.Lambda(
                                                                                       Code.Multiline()
                                                                                           .AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Local("connecting")).Close())
                                                                                           .AddLine(Code.Declare(Code.Generic("Subject", Code.Void()), "subject", Code.New(Code.Generic("Subject", Code.Void()))))
                                                                                           .AddLine(Code.Local("hubConnection").Method("start")
                                                                                                        .Method("then", Code.Lambda(Code.Multiline()
                                                                                                                                        .AddLine(Code.Local("subject").Method("next").Close())
                                                                                                                                        .AddLine(Code.Local("subject").Method("complete").Close())
                                                                                                                                        .AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Field("connected")).Close())))
                                                                                                        .Method("catch", Code.Lambda("error", errorCode)).Close())
                                                                                           .AddLine(Code.Return(Code.Local("subject")))
                                                                                   )))
                                                            .WithCode(createConnectionCode)
                                                            .WithCode(Code.Local("hubConnection").Method("onclose", Code.Lambda(Code.Multiline()
                                                                                                                                    .AddLine(Code.If(Code.Not().This().Field(isClosedField))
                                                                                                                                                 .WithCode(Code.Method("startConnection").Close())))
                                                                      ).Close())
                                                            .WithCode(Code.This().Field(connectionField).Method("next", Code.Local("hubConnection")).Close())
                                                            .WithCode(Code.Return(Code.Method("startConnection")));

                classTemplate.AddMethod("disconnect", Code.Void())
                             .WithComment("Close an active connection to the hub.\nIf the service is reconnecting/sleeping the connection attempt will be canceled")
                             .WithCode(Code.This().Field(isClosedField).Assign(Code.Boolean(true)).Close())
                             .WithCode(Code.This().Field(connectionField).NullConditional().Method("pipe", Code.Method("take", Code.Number(1)))
                                           .Method("subscribe", Code.Lambda("hubConnection", Code.Multiline()
                                                                                                 .AddLine(Code.Local("hubConnection").Method("stop").Method("then", Code.Lambda(Code.Multiline().AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local("ConnectionStatus").Field("disconnected")).Close()))).Close())
                                                   )).Close());

                if (timeoutsField != null)
                {
                    connectMethod.AddParameter(Code.Type("number"), "trial", Code.Number(0));
                }

                foreach (HttpServiceActionTransferObject action in hub.Actions)
                {
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", Code.Type("void")))
                                                                 .FormatName(hubOptions)
                                                                 .WithComment($"Send a \"{action.Name}\" message to the hub with the given parameters. Automatically connects to the hub.");
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (hubLanguage != null && outputLanguage != null)
                        {
                            this.MapType(hubLanguage, outputLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, hubOptions, relativeModelPath);
                        methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameter.Name, parameter.IsOptional ? Code.Null() : null).FormatName(hubOptions);
                    }

                    List<ICodeFragment> parameters = new List<ICodeFragment>();
                    parameters.Add(Code.String(action.Name));
                    parameters.AddRange(action.Parameters.Select(parameter => Code.Local(parameter.Name)));

                    string subjectName = action.Parameters.Any(x => x.Name == "subject") ? "rxjsSubject" : "subject";
                    methodTemplate.WithCode(Code.Declare(Code.Generic("Subject", Code.Void()), subjectName, Code.New(Code.Generic("Subject", Code.Void()))))
                                  .WithCode(
                                      Code.This().Method(connectMethod).Method("pipe",
                                                                               Code.Method("mergeMap", Code.Lambda(Code.This().Field(connectionField))),
                                                                               Code.Method("take", Code.Number(1)),
                                                                               Code.Method("mergeMap", Code.Lambda("connection", Code.Local("connection").Method("send", parameters))
                                                                               ))
                                          .Method("subscribe", Code.Lambda(Code.Multiline()
                                                                               .AddLine(Code.Local(subjectName).Method("next").Close())
                                                                               .AddLine(Code.Local(subjectName).Method("complete").Close())),
                                                  Code.Lambda("error", Code.Local(subjectName).Method("error", Code.Local("error")))).Close())
                                  .WithCode(Code.Return(Code.Local(subjectName)));
                }
                foreach (HttpServiceActionTransferObject action in hub.Events)
                {
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (hubLanguage != null && outputLanguage != null)
                        {
                            this.MapType(hubLanguage, outputLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, hubOptions, relativeModelPath);
                    }
                    TypeTemplate eventType;
                    List<ICodeFragment> eventResult = new List<ICodeFragment>();
                    if (action.Parameters.Count == 0)
                    {
                        eventType = Code.Void();
                    }
                    else if (action.Parameters.Count == 1)
                    {
                        eventType = action.Parameters.First().Type.ToTemplate();
                        eventResult.Add(Code.Local(action.Parameters.Single().Name));
                    }
                    else
                    {
                        AnonymousObjectTemplate anonymousObject = Code.AnonymousObject();
                        action.Parameters.ForEach(parameter => anonymousObject.AddProperty(parameter.Name).FormatName(hubOptions));
                        DeclareTypeTemplate declareTypeTemplate = namespaceTemplate.AddDeclareType(action.Name + "Event", anonymousObject).FormatName(hubOptions);
                        eventType = Code.Type(declareTypeTemplate.Name);
                        eventResult.Add(anonymousObject);
                    }
                    GenericTypeTemplate subjectType = Code.Generic("Subject", eventType);
                    FieldTemplate eventPrivateField = classTemplate.AddField(action.Name + "Subject", subjectType).Readonly().FormatName(hubOptions).Default(Code.New(subjectType));
                    FieldTemplate eventPublicField = classTemplate.AddField(action.Name + "$", Code.Generic("Observable", eventType)).Public().Readonly().FormatName(hubOptions)
                                                                  .Default(Code.This().Local(eventPrivateField).Method("asObservable"));
                    MultilineCodeFragment code = new MultilineCodeFragment();
                    code.AddLine(Code.This().Local(eventPrivateField).Method("next", eventResult.ToArray()).Close());
                    List<ICodeFragment> parameters = new List<ICodeFragment>();
                    parameters.Add(Code.String(action.Name));
                    parameters.Add(Code.Lambda(action.Parameters.Select(x => new ParameterTemplate(x.Type.ToTemplate(), x.Name)).ToList(), code));
                    createConnectionCode.AddLine(Code.Local("hubConnection").Method("on", parameters).Close());
                }
            }
        }

        private bool WriteDateFixes(ModelTransferObject model, bool isModelEnumerable, MultilineCodeFragment code, IReadOnlyList<string> chain, IReadOnlyList<TypeTransferObject> typeChain, List<ITransferObject> transferObjects)
        {
            if (model == null)
            {
                return false;
            }
            bool datePropertyFound = false;
            IfTemplate ifTemplate = Code.If(this.WriteFieldChain(chain));
            MultilineCodeFragment innerCode = ifTemplate.Code;
            if (isModelEnumerable)
            {
                innerCode = Code.Multiline();
                ifTemplate.WithCode(this.WriteFieldChain(chain).Method("forEach", Code.Lambda("entry", innerCode)).Close());
                chain = new List<string> { "entry" };
            }
            foreach (PropertyTransferObject property in model.Properties)
            {
                TypeTransferObject type = model.Generics.FirstOrDefault(generic => generic.Alias?.Name == property.Name)?.Type ?? property.Type;
                if (typeChain.Any(typeFromChain => typeFromChain.Name == property.Type.Name && typeFromChain.Namespace == property.Type.Namespace))
                {
                    continue;
                }
                IOptions propertyOptions = this.Options.Get(property);
                string propertyName = Formatter.FormatProperty(property.Name, propertyOptions);
                if (type.Name == nameof(DateTime))
                {
                    datePropertyFound = true;
                    if (isModelEnumerable)
                    {
                        innerCode.AddLine(Code.Local("entry").Field(propertyName).Assign(Code.This().Method("convertToDate", Code.Local("entry").Field(propertyName))).Close());
                    }
                    else
                    {
                        innerCode.AddLine(this.WriteFieldChain(chain).Field(propertyName).Assign(Code.This().Method("convertToDate", this.WriteFieldChain(chain).Field(propertyName))).Close());
                    }
                }
                ModelTransferObject propertyModel = type as ModelTransferObject;
                TypeTransferObject entryType = propertyModel?.Generics.FirstOrDefault()?.Type;
                entryType = entryType == null ? null : model.Generics.FirstOrDefault(generic => generic.Alias?.Name == entryType.Name)?.Type ?? entryType;
                ModelTransferObject entryModel = entryType as ModelTransferObject ?? transferObjects.OfType<ModelTransferObject>().FirstOrDefault(x => x.Name == entryType?.Name && x.Namespace == entryType?.Namespace);
                List<string> nextChain = new List<string>(chain);
                nextChain.Add(propertyName);
                List<TypeTransferObject> nextTypeChain = new List<TypeTransferObject>(typeChain);
                nextTypeChain.Add(type);
                if (propertyModel != null && propertyModel.IsEnumerable() && entryModel != null)
                {
                    datePropertyFound = this.WriteDateFixes(entryModel, true, innerCode, nextChain, nextTypeChain, transferObjects) || datePropertyFound;
                }
                else if (propertyModel != null && propertyModel.Properties.Count > 0)
                {
                    datePropertyFound = this.WriteDateFixes(propertyModel, false, innerCode, nextChain, nextTypeChain, transferObjects) || datePropertyFound;
                }
            }
            if (datePropertyFound)
            {
                code.AddLine(ifTemplate);
            }
            return datePropertyFound;
        }

        private ChainedCodeFragment WriteFieldChain(IEnumerable<string> chain)
        {
            List<string> chainList = chain.ToList();
            ChainedCodeFragment code = Code.Local(chainList.First());
            foreach (string field in chainList.Skip(1))
            {
                code = code.Field(field);
            }
            return code;
        }

        private void AppendConvertAnyMethod(ClassTemplate classTemplate)
        {
            classTemplate.AddMethod("convertAny", Code.Type("string"))
                         .WithParameter(Code.Type("any"), "value")
                         .WithCode(Code.Return(Code.InlineIf(Code.Local("value").Equals().ForceNull().Or().Local("value").Equals().Undefined(),
                                                             Code.String(string.Empty),
                                                             Code.Local("value").Method("toString")
                                               )
                                   ));
        }

        private void AppendConvertFromDateMethod(ClassTemplate classTemplate)
        {
            classTemplate.AddMethod("convertFromDate", Code.Type("string"))
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

        private void AppendConvertToDateMethod(ClassTemplate classTemplate)
        {
            classTemplate.AddMethod("convertToDate", Code.Type("Date"))
                         .WithParameter(Code.Type("string | Date"), "value")
                         .WithCode(Code.Return(Code.InlineIf(Code.TypeScript($"typeof(value) === \"string\""),
                                                             Code.New(Code.Type("Date"), Code.Local("value")),
                                                             Code.Local("value")
                                               )
                                   ));
        }

        private string GetAllowedName(ILanguage language, string name)
        {
            return language.ReservedKeywords.ContainsKey(name) ? language.ReservedKeywords[name] : name;
        }
    }
}
