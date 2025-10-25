using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.Sqlite.Commands;
using KY.Generator.Sqlite.Extensions;
using KY.Generator.Sqlite.Transfer.Readers;

namespace KY.Generator.Sqlite;

public class SqliteModule : ModuleBase
{
    public SqliteModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<SqliteReadDatabaseCommand>(SqliteReadDatabaseCommandParameters.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<SqliteWriteRepositoryCommand>(SqliteWriteRepositoryCommandParameters.Names);
        this.DependencyResolver.Bind<SqliteModelReader>().ToSelf();
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
