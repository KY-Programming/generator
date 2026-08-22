using System;
using System.Collections.Generic;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.TypeScript.Tests;

/// <summary>
/// The strict mode of the tsconfig.json next to the output is only a fallback for the strict-by-default behaviour,
/// see <see cref="TypeScriptStrictHelper" />.
/// </summary>
[TestClass]
public class TypeScriptStrictHelperTests
{
    private string basePath;
    private IDependencyResolver resolver;
    private Options options;

    [TestInitialize]
    public void Initialize()
    {
        Options.Register(() => new List<IOptionsFactory> { new GeneratorOptionsFactory(), new TypeScriptOptionsFactory() });
        // The global options are static, so a strict mode set by one test would decide the outcome of the next one
        Options.ClearGlobal();
        // The reader caches by path, so every test needs a directory of its own to stay independent of the others
        this.basePath = FileSystem.Combine(System.IO.Path.GetTempPath(), "KY.Generator.TypeScript.Tests", Guid.NewGuid().ToString("N"));
        FileSystem.CreateDirectory(FileSystem.Combine(this.basePath, "Output"));
        this.resolver = new DependencyResolver();
        this.options = new Options();
        this.resolver.Bind<Options>().To(this.options);
        this.resolver.Bind<List<ITransferObject>>().To(new List<ITransferObject>());
        this.resolver.Bind<IEnvironment>().To(new GeneratorEnvironment());
        this.resolver.Bind<IOutput>().To(new FileOutput(this.resolver.Get<IEnvironment>(), this.basePath));
    }

    [TestCleanup]
    public void Cleanup()
    {
        Options.ClearGlobal();
        FileSystem.DeleteDirectory(this.basePath);
    }

    [TestMethod]
    public void ReadsTheStrictModeOfTheTsConfigNextToTheOutput()
    {
        this.WriteTsConfig(@"{ ""compilerOptions"": { ""strict"": false } }");
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();

        typeScriptOptions.SetStrictFromConfig("Output", this.resolver);

        Assert.AreEqual(false, typeScriptOptions.StrictFromConfig);
        Assert.AreEqual(false, typeScriptOptions.Strict);
    }

    /// <summary>
    /// A strict mode that is set - by the typescript.strict of a ky-generator.json, an attribute or the fluent
    /// syntax - wins over the tsconfig.json anyway, so the file is not looked for at all
    /// </summary>
    [TestMethod]
    public void ASetStrictModeSkipsTheTsConfigCompletely()
    {
        this.WriteTsConfig(@"{ ""compilerOptions"": { ""strict"": false } }");
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();
        typeScriptOptions.Strict = true;

        typeScriptOptions.SetStrictFromConfig("Output", this.resolver);

        Assert.IsNull(typeScriptOptions.StrictFromConfig, "The tsconfig.json must not be read at all");
        Assert.AreEqual(true, typeScriptOptions.Strict);
    }

    /// <summary>
    /// The setting is read into the global options, which every narrower scope inherits from
    /// </summary>
    [TestMethod]
    public void AStrictModeSetOnTheGlobalOptionsAlsoSkipsIt()
    {
        this.WriteTsConfig(@"{ ""compilerOptions"": { ""strict"": false } }");
        Options.GetGlobal<TypeScriptOptions>().Strict = true;
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();

        typeScriptOptions.SetStrictFromConfig("Output", this.resolver);

        Assert.IsNull(typeScriptOptions.StrictFromConfig, "The tsconfig.json must not be read at all");
        Assert.AreEqual(true, typeScriptOptions.Strict);
    }

    [TestMethod]
    public void WithoutATsConfigTheStrictModeStaysUnset()
    {
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();

        typeScriptOptions.SetStrictFromConfig("Output", this.resolver);

        Assert.IsNull(typeScriptOptions.StrictFromConfig);
        Assert.AreEqual(true, typeScriptOptions.Strict, "Strict by default");
    }

    private void WriteTsConfig(string content)
    {
        FileSystem.WriteAllText(FileSystem.Combine(this.basePath, "Output", "tsconfig.json"), content);
    }
}
