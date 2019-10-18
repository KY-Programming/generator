using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Writers
{
    internal class AspDotNetEntityControllerWriter : Codeable
    {
        public void Write(AspDotNetWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            foreach (AspDotNetWriteEntityControllerConfiguration controllerConfiguration in configuration.Controllers)
            {
                EntityTransferObject entity = transferObjects.OfType<EntityTransferObject>().FirstOrDefault(x => x.Name == controllerConfiguration.Entity)
                                                             .AssertIsNotNull(nameof(controllerConfiguration.Entity), $"Entity {controllerConfiguration.Entity} not found. Ensure it is read before.");

                ClassTemplate controller = files.AddFile(configuration.RelativePath, configuration.AddHeader)
                                                .AddNamespace(controllerConfiguration.Namespace ?? configuration.Namespace)
                                                .AddClass(controllerConfiguration.Name ?? entity.Name + "Controller", Code.Type("ControllerBase"))
                                                .FormatName(configuration.Language, configuration.FormatNames)
                                                .WithAttribute("Route", Code.String(controllerConfiguration.Route ?? "[controller]"))
                                                .WithUsing("Microsoft.AspNetCore.Mvc")
                                                .WithUsing("System.Collections.Generic");

                TypeTemplate modelType = entity.Model.ToTemplate();

                configuration.Usings.ForEach(x => controller.AddUsing(x));
                controllerConfiguration.Usings.ForEach(x => controller.AddUsing(x));

                FieldTemplate repositoryField = controller.AddField("repository", Code.Type(entity.Name + "Repository")).Readonly();
                controller.AddConstructor().Code.AddLine(Code.This().Field(repositoryField).Assign(Code.New(repositoryField.Type)).Close());

                if (controllerConfiguration.Get != null)
                {
                    // TODO: Implement filters
                    controller.AddMethod("Get", Code.Generic("IEnumerable", modelType))
                              .WithAttribute("HttpGet", Code.String(controllerConfiguration.Get.Name ?? "[action]"))
                              .Code.AddLine(Code.Return(Code.This().Field(repositoryField).Method("Get")));
                }
                if (controllerConfiguration.Post != null)
                {
                    MethodTemplate method = controller.AddMethod("Post", Code.Void())
                                                      .WithAttribute("HttpPost", Code.String(controllerConfiguration.Post.Name ?? "[action]"));
                    ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                        .WithAttribute("FromBody");

                    method.Code.AddLine(Code.This().Field(repositoryField).Method("Add", Code.Local(parameter)).Close());
                }
                if (controllerConfiguration.Patch != null)
                {
                    MethodTemplate method = controller.AddMethod("Patch", Code.Void())
                                                      .WithAttribute("HttpPatch", Code.String(controllerConfiguration.Patch.Name ?? "[action]"));
                    ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                        .WithAttribute("FromBody");

                    method.Code.AddLine(Code.This().Field(repositoryField).Method("Update", Code.Local(parameter)).Close());
                }
                if (controllerConfiguration.Put != null)
                {
                    MethodTemplate method = controller.AddMethod("Put", Code.Void())
                                                      .WithAttribute("HttpPut", Code.String(controllerConfiguration.Put.Name ?? "[action]"));
                    ParameterTemplate parameter = method.AddParameter(modelType, "entity")
                                                        .WithAttribute("FromBody");

                    method.Code.AddLine(Code.This().Field(repositoryField).Method("Update", Code.Local(parameter)).Close());
                }
                if (controllerConfiguration.Delete != null)
                {
                    MethodTemplate method = controller.AddMethod("Delete", Code.Void())
                                                      .WithAttribute("HttpDelete", Code.String(controllerConfiguration.Delete.Name ?? "[action]"));
                    List<ParameterTemplate> parameters = new List<ParameterTemplate>();
                    foreach (string key in entity.Keys)
                    {
                        PropertyTransferObject property = entity.Model.Properties.First(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                        parameters.Add(method.AddParameter(property.Type.ToTemplate(), property.Name)
                                             .FormatName(configuration.Language, configuration.FormatNames));
                    }
                    method.Code.AddLine(Code.This().Field(repositoryField).Method("Delete", parameters.Select(x => Code.Local(x))).Close());
                }
            }
        }
    }
}