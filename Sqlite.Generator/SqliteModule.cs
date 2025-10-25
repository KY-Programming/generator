using KY.Core.Dependency;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Reflection;
using KY.Generator.Sqlite.Commands;
using KY.Generator.Sqlite.Extensions;
using KY.Generator.Sqlite.Transfer.Readers;

namespace KY.Generator.Sqlite;

public class SqliteModule : GeneratorModule
{
    public SqliteModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<ReflectionModule>();
        this.Register<SqliteReadDatabaseCommand>(SqliteReadDatabaseCommandParameters.Names);
        this.Register<SqliteWriteRepositoryCommand>(SqliteWriteRepositoryCommandParameters.Names);
        this.DependencyResolver.Bind<SqliteModelReader>().ToSelf();
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
