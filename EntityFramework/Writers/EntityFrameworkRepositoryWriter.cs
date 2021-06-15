using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Templates;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.EntityFramework.Writers
{
    public class EntityFrameworkRepositoryWriter : Codeable
    {
        public virtual void Write(EntityFrameworkWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            foreach (EntityFrameworkWriteRepositoryConfiguration repositoryConfiguration in configuration.Repositories)
            {
                EntityTransferObject entity = transferObjects.OfType<EntityTransferObject>().FirstOrDefault(x => x.Name == repositoryConfiguration.Entity)
                                                             .AssertIsNotNull(nameof(repositoryConfiguration.Entity), $"Entity {repositoryConfiguration.Entity} not found. Ensure it is read before.");

                ClassTemplate repository = files.AddFile(configuration.RelativePath, configuration.AddHeader, configuration.OutputId)
                                                .AddNamespace(repositoryConfiguration.Namespace ?? configuration.Namespace)
                                                .AddClass(repositoryConfiguration.Name ?? entity.Name + "Repository")
                                                .FormatName(configuration)
                                                .WithUsing("System.Collections.Generic")
                                                .WithUsing("System.Linq");
                if (configuration.IsCore)
                {
                    repository.WithUsing("Microsoft.EntityFrameworkCore");
                }
                else
                {
                    repository.WithUsing("System.Data.Entity");
                }
                if (!string.IsNullOrEmpty(configuration.Namespace) && !string.IsNullOrEmpty(repositoryConfiguration.Namespace) && configuration.Namespace != repositoryConfiguration.Namespace)
                {
                    repository.AddUsing(configuration.Namespace);
                }

                configuration.Usings.ForEach(x => repository.AddUsing(x));
                repositoryConfiguration.Usings.ForEach(x => repository.AddUsing(x));

                TypeTemplate modelType = entity.Model.ToTemplate();

                FieldTemplate dataSetField = repository.AddField("dataSet", Code.Generic("DbSet", modelType)).Readonly();
                FieldTemplate dataContextField = repository.AddField("dataContext", Code.Type("DataContext")).Readonly();

                TypeTemplate dataContextType = Code.Type("DataContext");
                ConstructorTemplate constructor = repository.AddConstructor();
                ParameterTemplate dataContextParameter = constructor.AddParameter(dataContextType, "dataContext", Code.Null());
                constructor.Code.AddLine(Code.This().Field(dataContextField).Assign(Code.NullCoalescing(Code.Local(dataContextParameter), Code.New(dataContextType))).Close())
                           .AddLine(Code.This().Field(dataSetField).Assign(Code.This().Field(dataContextField).GenericMethod("Set", modelType)).Close());

                repository.AddMethod("Get", Code.Generic("IQueryable", modelType))
                          .Code.AddLine(Code.Return(Code.This().Field(dataSetField)));

                repository.AddMethod("Get", modelType)
                          .WithParameter(Code.Type("params object[]"), "keys")
                          .Code.AddLine(Code.Return(Code.This().Field(dataSetField).Method("Find", Code.Local("keys"))));

                if (configuration.IsCore)
                {
                    repository.AddMethod("Add", modelType)
                              .WithParameter(modelType, "entity")
                              .Code.AddLine(Code.Declare(modelType, "result", Code.This().Field(dataSetField).Method("Add", Code.Local("entity")).Property("Entity")))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));

                    repository.AddMethod("Add", Code.Generic("IEnumerable", modelType))
                              .WithParameter(Code.Generic("IEnumerable", modelType), "entities")
                              .Code.AddLine(Code.Declare(Code.Generic("IEnumerable", modelType), "result", Code.Local("entities").Method("Select", Code.Lambda("x", Code.This().Field(dataSetField).Method("Add", Code.Local("x")).Property("Entity"))).Method("ToList")))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));

                    repository.AddMethod("Update", modelType)
                              .WithParameter(modelType, "entity")
                              .Code.AddLine(Code.Declare(modelType, "result", Code.This().Field(dataSetField).Method("Update", Code.Local("entity")).Property("Entity")))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));

                    repository.AddMethod("Update", Code.Generic("IEnumerable", modelType))
                              .WithParameter(Code.Generic("IEnumerable", modelType), "entities")
                              .Code.AddLine(Code.Declare(Code.Generic("IEnumerable", modelType), "result", Code.Local("entities").Method("Select", Code.Lambda("x", Code.This().Field(dataSetField).Method("Update", Code.Local("x")).Property("Entity"))).Method("ToList")))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));
                }
                else
                {
                    repository.AddMethod("Add", modelType)
                              .WithParameter(modelType, "entity")
                              .Code.AddLine(Code.Declare(modelType, "result", Code.This().Field(dataSetField).Method("Add", Code.Local("entity"))))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));

                    repository.AddMethod("Add", Code.Generic("IEnumerable", modelType))
                              .WithParameter(Code.Generic("IEnumerable", modelType), "entities")
                              .Code.AddLine(Code.Declare(Code.Generic("IEnumerable", modelType), "result", Code.Local("entities").Method("Select", Code.Lambda("x", Code.This().Field(dataSetField).Method("Add", Code.Local("x")))).Method("ToList")))
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));

                    repository.WithUsing("System.Data.Entity.Migrations")
                              .AddMethod("Update", modelType)
                              .WithParameter(modelType, "entity")
                              .Code.AddLine(Code.This().Field(dataSetField).Method("AddOrUpdate", Code.Local("entity")).Close())
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("entity")));

                    repository.WithUsing("System.Data.Entity.Migrations")
                              .AddMethod("Update", Code.Generic("IEnumerable", modelType))
                              .WithParameter(Code.Generic("IEnumerable", modelType), "entities")
                              .Code.AddLine(Code.Declare(Code.Generic("List", modelType), "result", Code.Local("entities").Method("ToList")))
                              .AddLine(Code.Local("result").Method("ForEach", Code.Lambda("x", Code.This().Field(dataSetField).Method("AddOrUpdate", Code.Local("x")))).Close())
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close())
                              .AddLine(Code.Return(Code.Local("result")));
                }

                //repository.AddMethod("Update", Code.Void())
                //          .WithParameter(Code.Generic("Delta", modelType), "delta")
                //          .WithParameter(Code.Type("object[]"), "keys")
                //          .Code.AddLine(Code.Declare(modelType, "entity", Code.This().Field(dataSetField).Method("Find", Code.Local("keys"))))
                //          .AddLine(Code.If(Code.Local("entity").Equals().Null(), x => x.Code.AddLine(Code.Throw(Code.Type("InvalidOperationException"), Code.String("Can not find any element with this keys, Use Add(...) method instead")))))
                //          .AddLine(Code.Local("delta").Method("Patch", Code.Local("entity")).Close())
                //          .AddLine(Code.This().Method("Update", Code.Local("entity")).Close())
                //          .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close());

                repository.AddMethod("Delete", Code.Void())
                          .WithParameter(modelType, "entity")
                          .Code.AddLine(Code.This().Field(dataSetField).Method("Remove", Code.Local("entity")).Close())
                          .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close());

                repository.AddMethod("Delete", Code.Void())
                          .WithParameter(Code.Generic("IEnumerable", modelType), "entities")
                          .Code.AddLine(Code.This().Field(dataSetField).Method("RemoveRange", Code.Local("entities")).Close())
                          .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close());

                if (configuration.IsCore)
                {
                    repository.AddMethod("Delete", Code.Void())
                              .WithParameter(Code.Type("params object[]"), "keys")
                              .Code.AddLine(Code.This().Field(dataSetField).Method("Remove", Code.This().Field(dataSetField).Method("Find", Code.Local("keys"))).Close())
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close());
                }
                else
                {
                    repository.AddMethod("Delete", Code.Void())
                              .WithParameter(Code.Type("params object[]"), "keys")
                              .Code.AddLine(Code.This().Field(dataSetField).Method("Remove", Code.This().Field(dataSetField).Method("Find", Code.Local("keys"))).Close())
                              .AddLine(Code.This().Field(dataContextField).Method("SaveChanges").Close());
                }

                //foreach (string key in entity.Keys)
                //{
                //    PropertyTransferObject property = entity.Model.Properties.First(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                //    delete.AddParameter(property.Type.ToTemplate(), property.Name)
                //          .FormatName(configuration.Language, configuration.FormatNames);
                //}
            }
        }
    }
}