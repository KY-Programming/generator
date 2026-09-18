using KY.Core.Dependency;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Commands;
using KY.Generator.Json.Transfers;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// A <c>preferInterfaces</c> from a <c>ky-generator.json</c> ends up in the global options and reaches the JSON
/// models as well. The reader of a model deserializes into the model itself, which Newtonsoft can not do for an
/// interface - so a model with reader has to stay a class
/// </summary>
[TestClass]
public class JsonWriteCommandTests
{
    private DependencyResolver resolver = null!;
    private List<ITransferObject> transferObjects = null!;
    private List<FileTemplate> files = null!;

    [TestInitialize]
    public void Initialize()
    {
        this.transferObjects = [];
        this.files = [];
        this.resolver = new DependencyResolver();
        this.resolver.Bind<Options>().ToSingleton();
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<List<ITransferObject>>().To(this.transferObjects);
        this.resolver.Bind<IList<FileTemplate>>().To(this.files);
        this.resolver.Bind<CsharpLanguage>().ToSingleton();
        this.resolver.Bind<IOutput>().To(new MemoryOutput());
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        Options.ClearGlobal();
        // What a ky-generator.json with "preferInterfaces": true does
        Options.GetGlobal<GeneratorOptions>().PreferInterfaces = true;

        JsonModelTransferObject model = new() { Name = "document", IsRoot = true };
        model.Properties.Add(new PropertyTransferObject { Name = "title", Type = new TypeTransferObject { Name = "string" }, DeclaringType = model });
        this.transferObjects.Add(model);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Options.ClearGlobal();
    }

    [TestMethod]
    public async Task ModelWithReaderIsAClassDespitePreferInterfaces()
    {
        ClassTemplate classTemplate = await this.Write(withReader: true);

        Assert.IsFalse(classTemplate.IsInterface, "The reader can not deserialize into an interface");
        Assert.AreEqual("Translation", classTemplate.Name, "A class must not get the interface prefix");
    }

    [TestMethod]
    public async Task ModelWithoutReaderFollowsPreferInterfaces()
    {
        ClassTemplate classTemplate = await this.Write(withReader: false);

        Assert.IsTrue(classTemplate.IsInterface, "Without reader nothing speaks against an interface");
        Assert.AreEqual("ITranslation", classTemplate.Name);
    }

    private async Task<ClassTemplate> Write(bool withReader)
    {
        JsonWriteCommand command = this.resolver.Create<JsonWriteCommand>();
        command.Parameters.ModelName = "Translation";
        command.Parameters.ModelNamespace = "KY.Test";
        command.Parameters.RelativePath = "Output";
        command.Parameters.WithReader = withReader;
        command.Prepare();
        await command.Run();

        return this.files.SelectMany(file => file.Namespaces)
                   .SelectMany(ns => ns.Children)
                   .OfType<ClassTemplate>()
                   .Single();
    }
}
