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

namespace KY.Generator.EntityFramework.Writers
{
    internal class EntityFrameworkDataContextWriter : Codeable
    {
        public void Write(EntityFrameworkWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            ClassTemplate dataContext = files.AddFile(configuration.RelativePath)
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
                           .FormatName(configuration.Language, configuration.FormatNames)
                           .Virtual();
            }

            ConstructorTemplate constructor = dataContext.AddConstructor();
            ParameterTemplate connectionString = constructor.AddParameter(Code.Type("string"), "connectionString", Code.Null());
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
                IEnumerable<ICodeFragment> keyNames = entity.Keys.Select(key => Code.String(key));
                createMethod.Code.AddLine(Code.Local(modelBuilder).GenericMethod("Entity", entity.Model.ToTemplate()).BreakLine()
                                              .Method("ToTable", Code.String(entity.Table), Code.String(entity.Schema)).BreakLine()
                                              .Method("HasKey", keyNames).Close());
            }
        }
    }
}