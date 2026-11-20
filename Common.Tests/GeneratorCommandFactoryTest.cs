using System.Collections.Generic;
using System.Linq;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Common.Tests;

[TestClass]
public class GeneratorCommandFactoryTest
{
    [TestMethod]
    public void GlobalParametersAreAddedToEveryCommand()
    {
        GeneratorEnvironment environment = new();
        environment.Parameters.AddRange(CliCommandReader.ReadGlobal("-*global=everywhere"));
        DependencyResolver resolver = new();
        resolver.Bind<IEnvironment>().To(environment);
        GeneratorCommandFactory factory = new(resolver);
        factory.Register<VersionCommand>(VersionCommandParameters.Names);

        List<IGeneratorCommand> commands = factory.Create(CliCommandReader.Read("version", "-d", "version"), resolver);

        Assert.AreEqual(2, commands.Count);
        CollectionAssert.AreEqual(new[] { "d", "global" }, commands[0].OriginalParameters.Select(x => x.Name).ToList());
        CollectionAssert.AreEqual(new[] { "global" }, commands[1].OriginalParameters.Select(x => x.Name).ToList());
        Assert.IsTrue(commands.All(command => command.OriginalParameters.Last().Value == "everywhere"));
    }
}
