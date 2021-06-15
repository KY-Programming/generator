using System.Collections.Generic;
using System.Linq;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Templates;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;
using KY.Generator.Tsql.Transfers;

namespace KY.Generator.EntityFramework.Writers
{
    public class EntityFrameworkDataContextWriter : Codeable
    {
        public virtual void Write(EntityFrameworkWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            this.WriteClass(configuration, transferObjects, files);
        }

        protected virtual ClassTemplate WriteClass(EntityFrameworkWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            ClassTemplate dataContext = files.AddFile(configuration.RelativePath, configuration.AddHeader, configuration.OutputId)
                                               .AddNamespace(configuration.Namespace)
                                               .AddClass("DataContext", Code.Type("DbContext"));

            if (configuration.IsCore)
            {
                dataContext.WithUsing("Microsoft.EntityFrameworkCore");
            }
            else
            {
                dataContext.WithUsing("System.Data.Entity");
            }
                
            configuration.Usings.ForEach(x => dataContext.AddUsing(x));

            PropertyTemplate defaultConnectionProperty = dataContext.AddProperty("DefaultConnection", Code.Type("string")).Static().WithDefaultValue(Code.String("name=DataContext"));

            foreach (EntityTransferObject entity in transferObjects.OfType<EntityTransferObject>())
            {
                dataContext.AddProperty(entity.Name, Code.Generic("DbSet", entity.Model.ToTemplate()))
                           .FormatName(configuration)
                           .Virtual();
            }

            dataContext.AddConstructor()
                       .WithThisConstructor(Code.Null());

            ConstructorTemplate constructor = dataContext.AddConstructor();
            ParameterTemplate connectionString = constructor.AddParameter(Code.Type("string"), "connectionString");
            if (configuration.IsCore)
            {
                constructor.WithBaseConstructor(Code.Static(Code.Type("SqlServerDbContextOptionsExtensions")).Method("UseSqlServer", Code.New(Code.Type("DbContextOptionsBuilder")), Code.NullCoalescing(Code.Local(connectionString), Code.Local(defaultConnectionProperty))).Property("Options"))
                .Code.AddLine(Code.This().Property("Database").Method("SetCommandTimeout", Code.Number(configuration.DataContext.CommandTimeout)).Close());
            }
            else
            {
                constructor.WithBaseConstructor(Code.NullCoalescing(Code.Local("connectionString"), Code.Local(defaultConnectionProperty)))
                .Code.AddLine(Code.This().Property("Database").Property("CommandTimeout").Assign(Code.Number(configuration.DataContext.CommandTimeout)).Close());
            }

            MethodTemplate createMethod = dataContext.AddMethod("OnModelCreating", Code.Void()).Protected().Override();
            ParameterTemplate modelBuilder = createMethod.AddParameter(Code.Type(configuration.IsCore ? "ModelBuilder" : "DbModelBuilder"), "modelBuilder");
            if (!configuration.IsCore)
            {
                createMethod.Code.AddLine(Code.Local(modelBuilder).Property("Configurations").Method("AddFromAssembly", Code.This().Method("GetType").Property("Assembly")).Close());
            }

            foreach (EntityTransferObject entity in transferObjects.OfType<EntityTransferObject>())
            {
                createMethod.Code.AddLine(Code.Local(modelBuilder).GenericMethod("Entity", entity.Model.ToTemplate()).BreakLine()
                                              .Method("ToTable", Code.String(entity.Table), Code.String(entity.Schema)).BreakLine()
                                              .Method("HasKey", Code.Lambda("x", Code.Csharp("new { " + string.Join(", ", entity.Keys.Select(key => $"x.{key.Name}")) + " }" ))).Close());
            }
            foreach (StoredProcedureTransferObject storedProcedure in transferObjects.OfType<StoredProcedureTransferObject>())
            {
                dataContext.AddMethod(storedProcedure.Name, storedProcedure.ReturnType.ToTemplate())
                           .Code.AddLine(Code.This().Property("Database").Method("ExecuteSqlCommand", Code.String($"exec {storedProcedure.Schema}.{storedProcedure.Name}")).Close());
            }
            return dataContext;
        }
    }
}