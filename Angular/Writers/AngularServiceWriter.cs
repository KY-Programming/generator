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
                                                  .FormatName(configuration, true)
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
                relativeModelPath = string.IsNullOrEmpty(relativeModelPath) ? "." : relativeModelPath;
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
                    if (returnType.Name == "Task")
                    {
                        returnType = new TypeTemplate("void");
                    }
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
                    string uri = ("/" + (controller.Route?.Replace("[controller]", controllerName.ToLower()).TrimEnd('/') ?? controllerName.ToLower()) + "/" + action.Route?.Replace("[action]", action.Name.ToLower())).TrimEnd('/');

                    List<HttpServiceActionParameterTransferObject> inlineParameters = action.Parameters.Where(x => !x.FromBody && x.Inline).OrderBy(x => x.InlineIndex).ToList();
                    List<HttpServiceActionParameterTransferObject> urlParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && x.AppendName).ToList();
                    List<HttpServiceActionParameterTransferObject> urlDirectParameters = action.Parameters.Where(x => !x.FromBody && !x.Inline && !x.AppendName).ToList();
                    MultilineCodeFragment code = Code.Multiline();
                    bool hasReturnType = returnType.Name != "void" && returnType.Name != "Task";
                    ExecuteMethodTemplate nextMethod = Code.Local("subject").Method("next");
                    if (hasReturnType)
                    {
                        nextMethod.WithParameter(Code.Local("result"));
                    }
                    code.AddLine(nextMethod.Close())
                        .AddLine(Code.Local("subject").Method("complete").Close());
                    ChainedCodeFragment parameterUrl = Code.This().Field(serviceUrlField);
                    foreach (HttpServiceActionParameterTransferObject parameter in inlineParameters)
                    {
                        string[] chunks = uri.Split(new[] { $"{{{parameter.Name}}}" }, StringSplitOptions.RemoveEmptyEntries);
                        parameterUrl = parameterUrl.Append(Code.String(chunks[0])).Append(Code.Local(parameter.Name));
                        uri = chunks.Length == 1 ? string.Empty : chunks[1];
                    }
                    if (!string.IsNullOrEmpty(uri))
                    {
                        parameterUrl = parameterUrl.Append(Code.String(uri));
                    }
                    bool isFirst = true;
                    if (controller.Version != null)
                    {
                        isFirst = false;
                        parameterUrl = parameterUrl.Append(Code.String($"?api-version={controller.Version}"));
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
                        string name = mapping[parameter].Name;
                        parameterUrl = parameterUrl.Append(isFirst ? Code.String($"?{parameter.Name}=") : Code.String($"&{parameter.Name}="));
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
                    this.AppendConvertAnyMethod(classTemplate);
                }
                if (appendConvertDateMethod)
                {
                    this.AppendConvertDateMethod(classTemplate);
                }
            }
            List<SignalRHubTransferObject> hubs = transferObjects.OfType<SignalRHubTransferObject>().ToList();
            FileTemplate connectionStatusFileTemplate = null;
            EnumTemplate connectionStatusEnum = null;
            if (hubs.Count > 0)
            {
                connectionStatusFileTemplate = files.AddFile(configuration.Model.RelativePath);
                connectionStatusEnum = connectionStatusFileTemplate
                                       .AddNamespace(string.Empty)
                                       .AddEnum("ConnectionStatus")
                                       .FormatName(configuration)
                                       .AddValue("connecting")
                                       .AddValue("connected")
                                       .AddValue("disconnected");
            }
            foreach (SignalRHubTransferObject hub in hubs)
            {
                string relativeModelPath = FileSystem.RelativeTo(configuration.Model?.RelativePath ?? ".", configuration.Service.RelativePath);
                IMappableLanguage hubLanguage = hub.Language as IMappableLanguage;
                IMappableLanguage configurationLanguage = configuration.Language as IMappableLanguage;
                FileTemplate file = files.AddFile(configuration.Service.RelativePath, configuration.AddHeader, configuration.CheckOnOverwrite);
                ClassTemplate classTemplate = file.AddNamespace(string.Empty)
                                                  .AddClass(configuration.Service.Name?.Replace("{0}", hub.Name) ?? hub.Name + "Service")
                                                  .FormatName(configuration, true)
                                                  .WithUsing("Injectable", "@angular/core")
                                                  .WithUsing("Subject", "rxjs")
                                                  .WithUsing("Observable", "rxjs")
                                                  .WithUsing("ReplaySubject", "rxjs")
                                                  .WithUsing("flatMap", "rxjs/operators")
                                                  .WithUsing("of", "rxjs")
                                                  .WithUsing("HubConnectionBuilder", "@aspnet/signalr")
                                                  .WithUsing("HubConnectionState", "@aspnet/signalr")
                                                  .WithUsing("HubConnection", "@aspnet/signalr")
                                                  .WithUsing(connectionStatusEnum.Name, FileSystem.Combine(relativeModelPath, Formatter.FormatFile(connectionStatusFileTemplate.Name, configuration, true)).Replace("\\", "/"))
                                                  .WithAttribute("Injectable", Code.AnonymousObject().WithProperty("providedIn", Code.String("root")));
                FieldTemplate serviceUrlField = classTemplate.AddField("serviceUrl", Code.Type("string")).Public().FormatName(configuration).Default(Code.String(string.Empty));
                FieldTemplate connectionField = classTemplate.AddField("connection", Code.Type("HubConnection"));
                FieldTemplate timeoutsField = null;
                if (configuration.Service.Timeouts?.Count > 0)
                {
                    timeoutsField = classTemplate.AddField("timeouts", Code.Generic("Array", Code.Type("number"))).Readonly()
                                                 .Default(Code.TypeScript($"[{string.Join(", ", configuration.Service.Timeouts)}]"));
                }
                FieldTemplate statusSubjectField = classTemplate.AddField("statusSubject", Code.Generic("ReplaySubject", connectionStatusEnum.ToType())).Readonly()
                                                                .Default(Code.New(Code.Generic("ReplaySubject", connectionStatusEnum.ToType()), Code.Number(1)));
                classTemplate.AddField("status$", Code.Generic("Observable", connectionStatusEnum.ToType())).FormatName(configuration).Readonly().Public()
                             .Default(Code.This().Local(statusSubjectField).Method("asObservable"));
                MultilineCodeFragment createConnectionCode = Code.Multiline().AddLine(
                    Code.This().Field(connectionField).Assign(Code.New(Code.Type("HubConnectionBuilder")).Method("withUrl", Code.This().Local(serviceUrlField)).Method("build")).Close()
                ).AddLine(
                    Code.This().Field(connectionField).Method("onclose", Code.Lambda(Code.Multiline()
                                                                                         .AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Local("connecting")).Close())
                                                                                         .AddLine(Code.This().Method("connect")))
                    ).Close()
                );
                MultilineCodeFragment errorCode = Code.Multiline().AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Field("disconnected")).Close());
                if (timeoutsField != null)
                {
                    errorCode.AddLine(Code.Declare(Code.Type("number"), "timeout", Code.This().Field(timeoutsField).Index(Code.Local("trial"))));
                    if (configuration.Service.EndlessTries)
                    {
                        errorCode.AddLine(Code.Local("timeout").Assign(Code.Local("timeout").Or().This().Field(timeoutsField).Index(Code.This().Field(timeoutsField).Field("length").Subtract().Number(1)).Or().Number(0)).Close());
                    }
                    else
                    {
                        errorCode.AddLine(Code.If(Code.Local("timeout").Equals().Undefined()).WithCode(Code.Local("subject").Method("error", Code.Local("error")).Close()).WithCode(Code.Return()));
                    }
                    errorCode.AddLine(Code.Method("setTimeout",
                                                  Code.Lambda(Code.This().Method("connect", Code.Local("trial").Add().Number(1)).Method("subscribe",
                                                                                                                                        Code.Lambda(Code.Multiline()
                                                                                                                                                        .AddLine(Code.Local("subject").Method("next").Close())
                                                                                                                                                        .AddLine(Code.Local("subject").Method("complete").Close())),
                                                                                                                                        Code.Lambda("innerError", Code.Local("subject").Method("error", Code.Local("innerError")))
                                                              )),
                                                  Code.Local("timeout")
                                      ).Close());
                }
                else
                {
                    errorCode.AddLine(Code.Local("subject").Method("error", Code.Local("error")).Close());
                }

                MethodTemplate connectMethod = classTemplate.AddMethod("Connect", Code.Generic("Observable", Code.Void())).FormatName(configuration)
                                                            .WithCode(Code.If(Code.Not().This().Local(serviceUrlField))
                                                                          .WithCode(Code.Throw(Code.Type("Error"), Code.String("serviceUrl can not be empty. Set it via service.serviceUrl."))))
                                                            .WithCode(Code.If(Code.Not().This().Field(connectionField))
                                                                          .WithCode(createConnectionCode))
                                                            .WithCode(Code.If(Code.This().Field(connectionField).Field("state").Equals().Local("HubConnectionState").Local("Connected"))
                                                                          .WithCode(Code.Return(Code.Method("of", Code.Undefined()))))
                                                            .WithCode(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Local("connecting")).Close())
                                                            .WithCode(Code.Declare(Code.Generic("Subject", Code.Void()), "subject", Code.New(Code.Generic("Subject", Code.Void()))))
                                                            .WithCode(Code.This().Field(connectionField).Method("start")
                                                                          .Method("then", Code.Lambda(Code.Multiline()
                                                                                                          .AddLine(Code.Local("subject").Method("next").Close())
                                                                                                          .AddLine(Code.Local("subject").Method("complete").Close())
                                                                                                          .AddLine(Code.This().Field(statusSubjectField).Method("next", Code.Local(connectionStatusEnum.Name).Field("connected")).Close())))
                                                                          .Method("catch", Code.Lambda("error", errorCode)).Close())
                                                            .WithCode(Code.Return(Code.Local("subject")));
                if (timeoutsField != null)
                {
                    connectMethod.AddParameter(Code.Type("number"), "trial", Code.Number(0));
                }

                foreach (HttpServiceActionTransferObject action in hub.Actions)
                {
                    MethodTemplate methodTemplate = classTemplate.AddMethod(action.Name, Code.Generic("Observable", Code.Type("void")))
                                                                 .FormatName(configuration);
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (hubLanguage != null && configurationLanguage != null)
                        {
                            this.MapType(hubLanguage, configurationLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, configuration, relativeModelPath);
                        methodTemplate.AddParameter(parameter.Type.ToTemplate(), parameter.Name, parameter.IsOptional ? Code.Null() : null).FormatName(configuration);
                    }

                    List<ICodeFragment> parameters = new List<ICodeFragment>();
                    parameters.Add(Code.String(action.Name));
                    parameters.AddRange(action.Parameters.Select(parameter => Code.Local(parameter.Name)));

                    methodTemplate.WithCode(Code.Declare(Code.Generic("Subject", Code.Void()), "subject", Code.New(Code.Generic("Subject", Code.Void()))))
                                  .WithCode(
                                      Code.This().Method(connectMethod).Method("pipe", Code.Method("flatMap", Code.Lambda(Code.This().Field(connectionField).Method("send", parameters))))
                                          .Method("subscribe", Code.Lambda(Code.Multiline()
                                                                               .AddLine(Code.Local("subject").Method("next").Close())
                                                                               .AddLine(Code.Local("subject").Method("complete").Close())),
                                                  Code.Lambda("error", Code.Local("subject").Method("error", Code.Local("error")))).Close())
                                  .WithCode(Code.Return(Code.Local("subject")));
                }
                foreach (HttpServiceActionTransferObject action in hub.Events)
                {
                    if (action.Parameters.Count > 1)
                    {
                        Logger.Error("SignalR hub event: More than one parameter is currently not implemented");
                        continue;
                    }
                    foreach (HttpServiceActionParameterTransferObject parameter in action.Parameters)
                    {
                        if (hubLanguage != null && configurationLanguage != null)
                        {
                            this.MapType(hubLanguage, configurationLanguage, parameter.Type);
                        }
                        this.AddUsing(parameter.Type, classTemplate, configuration, relativeModelPath);
                    }
                    TypeTemplate eventType = action.Parameters.Single().Type.ToTemplate();
                    GenericTypeTemplate subjectType = Code.Generic("Subject", eventType);
                    FieldTemplate eventPrivateField = classTemplate.AddField(action.Name + "Subject", subjectType).Readonly().FormatName(configuration).Default(Code.New(subjectType));
                    FieldTemplate eventPublicField = classTemplate.AddField(action.Name + "$", Code.Generic("Observable", eventType)).Public().Readonly().FormatName(configuration)
                                                                  .Default(Code.This().Local(eventPrivateField).Method("asObservable"));
                    MultilineCodeFragment code = new MultilineCodeFragment();
                    code.AddLine(Code.This().Local(eventPrivateField).Method("next", Code.Local(action.Parameters.Single().Name)).Close());
                    List<ICodeFragment> parameters = new List<ICodeFragment>();
                    parameters.Add(Code.String(action.Name));
                    parameters.Add(Code.Lambda(action.Parameters.Select(x => new ParameterTemplate(x.Type.ToTemplate(), x.Name)).ToList(), code));
                    createConnectionCode.AddLine(Code.This().Local(connectionField).Method("on", parameters).Close());
                }
            }
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

        private void AppendConvertDateMethod(ClassTemplate classTemplate)
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