using KY.Core;
using KY.Generator.Angular;
using KY.Generator.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// More than one <see cref="Generator"/> in a process - the fluent API, a host that generates repeatedly, a test run.
/// The first one of a process finds no module assembly loaded yet, every later one finds all of them already loaded,
/// which is the case that used to register every command twice and end in a stack overflow
/// </summary>
[TestClass]
public class GeneratorInstanceTests
{
    private readonly List<string> errors = [];

    [TestInitialize]
    public void Initialize()
    {
        Logger.Added += this.OnLoggerAdded;
    }

    [TestCleanup]
    public void Cleanup()
    {
        Logger.Added -= this.OnLoggerAdded;
        this.errors.Clear();
    }

    [TestMethod]
    public void ASecondGeneratorInTheSameProcessLoadsItsModulesOnce()
    {
        // The Angular module depends on the TypeScript module in its constructor - having it loaded before the
        // generators are created is what made the module finder start over while it was still being built
        Assert.IsNotNull(typeof(AngularModule).Assembly);

        Generator.Create();
        Generator second = Generator.Create();

        CollectionAssert.AreEqual(new List<string>(), this.errors, "Creating the generators must not log errors");
        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(() => second.RegisterCommand<VersionCommand>(VersionCommandParameters.Names),
                                                                                                  "The core module of the second generator has to have registered its commands");
        StringAssert.Contains(exception.Message, "already registered");
    }

    private void OnLoggerAdded(object? sender, EventArgs<LogEntry> args)
    {
        if (args.Value.Type == LogType.Error)
        {
            this.errors.Add(args.Value.Message);
        }
    }
}
