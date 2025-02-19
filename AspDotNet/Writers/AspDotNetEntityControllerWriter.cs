﻿using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Writers;

public class AspDotNetEntityControllerWriter : Codeable
{
    private readonly Options options;
    private readonly List<ITransferObject> transferObjects;
    private readonly List<FileTemplate> files;

    public AspDotNetEntityControllerWriter(Options options, List<ITransferObject> transferObjects, List<FileTemplate> files)
    {
        this.options = options;
        this.transferObjects = transferObjects;
        this.files = files;
    }

    public virtual void Write(AspDotNetWriteConfiguration configuration)
    {
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        if (!generatorOptions.Language.IsCsharp())
        {
            throw new InvalidOperationException($"Can not generate ASP.net Controller for language {generatorOptions.Language?.Name ?? "Empty"}. Only Csharp is currently implemented");
        }
        foreach (AspDotNetWriteEntityControllerConfiguration controllerConfiguration in configuration.Controllers)
        {
            EntityTransferObject entity = this.transferObjects.OfType<EntityTransferObject>().FirstOrDefault(x => x.Name == controllerConfiguration.Entity)
                                              .AssertIsNotNull(nameof(controllerConfiguration.Entity), $"Entity {controllerConfiguration.Entity} not found. Ensure it is read before.");
            GeneratorOptions entityOptions = this.options.Get<GeneratorOptions>(entity);
            string nameSpace = (controllerConfiguration.Namespace ?? configuration.Namespace).AssertIsNotNull(nameof(configuration.Namespace), "asp writer requires a namespace");
            string className = controllerConfiguration.Name ?? entity.Name + "Controller";
            ClassTemplate controller = this.files.AddFile(configuration.RelativePath, entityOptions)
                                           .WithName(Formatter.FormatFile(className, entityOptions))
                                           .AddNamespace(nameSpace)
                                           .AddClass(className, Code.Type(configuration.Template.ControllerBase))
                                           .FormatName(entityOptions)
                                           .FormatPrefix(entityOptions)
                                           .WithAttribute("Route", Code.String(controllerConfiguration.Route ?? "[controller]"));

            controller.Usings.AddRange(configuration.Template.Usings);

            TypeTemplate modelType = entity.Model.ToTemplate();

            configuration.Usings.ForEach(x => controller.AddUsing(x));
            controllerConfiguration.Usings.ForEach(x => controller.AddUsing(x));

            FieldTemplate repositoryField = controller.AddField("repository", Code.Type(entity.Name + "Repository")).Readonly();
            controller.AddConstructor().Code.AddLine(Code.This().Field(repositoryField).Assign(Code.New(repositoryField.Type)).Close());

            if (controllerConfiguration.Get != null)
            {
                controller.AddUsing("System.Linq");
                MethodTemplate method = controller.AddMethod("Get", Code.Generic("IEnumerable", modelType));
                if (configuration.Template.UseAttributes)
                {
                    method.WithAttribute("HttpGet", Code.String(controllerConfiguration.Get.Name ?? "[action]"));
                }
                DeclareTemplate queryable = Code.Declare(Code.Generic("IQueryable", modelType), "queryable", Code.This().Field(repositoryField).Method("Get"));
                method.Code.AddLine(queryable);
                foreach (PropertyTransferObject property in entity.Model.Properties)
                {
                    ParameterTemplate parameter = method.AddParameter(property.Type.ToTemplate(), property.Name, Code.Local("default")) /*.FormatName(configuration)*/;
                    method.Code.AddLine(Code.If(Code.Local(parameter).NotEquals().Local("default"), x => x.Code.AddLine(Code.Local(queryable).Assign(Code.Local(queryable).Method("Where", Code.Lambda("x", Code.Local("x").Property(property.Name).Equals().Local(parameter)))).Close())));
                }
                method.Code.AddLine(Code.Return(Code.Local(queryable)));
            }
            if (controllerConfiguration.Post != null)
            {
                MethodTemplate method = controller.AddMethod("Post", Code.Void());
                if (configuration.Template.UseAttributes)
                {
                    method.WithAttribute("HttpPost", Code.String(controllerConfiguration.Post.Name ?? "[action]"));
                }
                ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                    .WithAttribute("FromBody");

                method.Code.AddLine(Code.This().Field(repositoryField).Method("Add", Code.Local(parameter)).Close());
            }
            if (controllerConfiguration.Patch != null)
            {
                MethodTemplate method = controller.AddMethod("Patch", Code.Void());
                if (configuration.Template.UseAttributes)
                {
                    method.WithAttribute("HttpPatch", Code.String(controllerConfiguration.Patch.Name ?? "[action]"));
                }
                ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                    .WithAttribute("FromBody");

                method.Code.AddLine(Code.This().Field(repositoryField).Method("Update", Code.Local(parameter)).Close());
            }
            if (controllerConfiguration.Put != null)
            {
                MethodTemplate method = controller.AddMethod("Put", Code.Void());
                if (configuration.Template.UseAttributes)
                {
                    method.WithAttribute("HttpPut", Code.String(controllerConfiguration.Put.Name ?? "[action]"));
                }
                ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                    .WithAttribute("FromBody");

                method.Code.AddLine(Code.This().Field(repositoryField).Method("Update", Code.Local(parameter)).Close());
            }
            if (controllerConfiguration.Delete != null)
            {
                MethodTemplate method = controller.AddMethod("Delete", Code.Void());
                if (configuration.Template.UseAttributes)
                {
                    method.WithAttribute("HttpDelete", Code.String(controllerConfiguration.Delete.Name ?? "[action]"));
                }
                List<ParameterTemplate> parameters = new();
                foreach (EntityKeyTransferObject key in entity.Keys)
                {
                    PropertyTransferObject property = entity.Model.Properties.First(x => x.Name.Equals(key.Name, StringComparison.InvariantCultureIgnoreCase));
                    GeneratorOptions propertyOptions = this.options.Get<GeneratorOptions>(property);
                    parameters.Add(method.AddParameter(property.Type.ToTemplate(), property.Name)
                                         .FormatName(propertyOptions));
                }
                method.Code.AddLine(Code.This().Field(repositoryField).Method("Delete", parameters.Select(x => Code.Local(x))).Close());
            }
        }
    }
}
