using KY.Core.Dependency;
using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Reflection.Readers;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// Whether a model is written as class or interface. A <c>preferInterfaces</c> from a <c>ky-generator.json</c> applies to
/// every model of the run. <see cref="GeneratePreferClassesAttribute"/> turns it around for a type and everything
/// generated through it, <see cref="GenerateClassAttribute.ForceClass"/> and
/// <see cref="GenerateClassAttribute.ForceInterface"/> pin one single type and fail when they can not be honored
/// </summary>
[TestClass]
public class InterfaceOrClassTests
{
    public class Detail
    {
        public string Name { get; set; } = "";
    }

    public class PlainModel
    {
        public Detail Detail { get; set; } = new();
    }

    [GeneratePreferClasses]
    public class PreferClassesModel
    {
        public Detail Detail { get; set; } = new();
    }

    [GeneratePreferInterfaces]
    public class PreferInterfacesModel
    { }

    [GenerateClass(ForceClass = true)]
    public class ForceClassModel
    {
        public Detail Detail { get; set; } = new();
    }

    [GenerateClass(ForceInterface = true)]
    public class ForceInterfaceDetail
    {
        public string Name { get; set; } = "";
    }

    [GeneratePreferClasses]
    public class PreferClassesWithForcedInterface
    {
        public ForceInterfaceDetail Detail { get; set; } = new();
    }

    [GenerateClass(Replace = "Model")]
    [GenerateClass(ForceClass = true)]
    public class RenamedForceClassModel
    { }

    [GenerateClass(ForceClass = true)]
    [GenerateClass(ForceInterface = true)]
    public class ForceBothModel
    { }

    [GenerateClass(ForceInterface = true)]
    public class ForceInterfaceWithConstant
    {
        public const string Constant = "value";
    }

    public class PlainModelWithConstant
    {
        public const string Constant = "value";
    }

    [GenerateClass(ForceClass = true)]
    public interface IForceClassInterface
    { }

    [GenerateClass(ForceClass = true)]
    public enum ForceClassEnum
    {
        Value
    }

    private DependencyResolver resolver = null!;
    private List<FileTemplate> files = null!;

    [TestInitialize]
    public void Initialize()
    {
        this.files = [];
        GeneratorEnvironment environment = new();
        environment.LoadedAssemblies.Add(typeof(InterfaceOrClassTests).Assembly);
        this.resolver = new DependencyResolver();
        this.resolver.Bind<Options>().ToSingleton();
        this.resolver.Bind<IEnvironment>().To(environment);
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<List<ITransferObject>>().To(new List<ITransferObject>());
        this.resolver.Bind<IList<FileTemplate>>().To(this.files);
        this.resolver.Bind<GeneratorTypeLoader>().ToSingleton();
        this.resolver.Bind<ReflectionModelReader>().ToSingleton();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        this.resolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        Options.Register(() => this.resolver.Get<List<IOptionsFactory>>());
        Options.ClearGlobal();
        // What a ky-generator.json with "preferInterfaces": true does
        Options.GetGlobal<GeneratorOptions>().PreferInterfaces = true;
        Options.GetGlobal<GeneratorOptions>().Language = new TypeScriptLanguage(this.resolver);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Options.ClearGlobal();
    }

    [TestMethod]
    public void PlainModelFollowsPreferInterfaces()
    {
        this.Write<PlainModel>();

        Assert.IsTrue(this.Get(nameof(PlainModel)).IsInterface);
        Assert.IsTrue(this.Get(nameof(Detail)).IsInterface);
    }

    [TestMethod]
    public void ModelWithConstantsStaysAClassWithoutInterfacePrefix()
    {
        // C# is the language with an interface prefix ("I"), TypeScript has none and would not show a wrong one
        Options.GetGlobal<GeneratorOptions>().Language = new CsharpLanguage(this.resolver);
        this.resolver.Get<ReflectionModelReader>().Read(typeof(PlainModelWithConstant));
        this.resolver.Create<ModelWriter>().FormatNames().Write("Output");

        ClassTemplate model = this.Get(nameof(PlainModelWithConstant));
        Assert.IsFalse(model.IsInterface, "An interface can not carry the value of a constant");
        Assert.AreEqual(nameof(PlainModelWithConstant), model.Name, "A class must not get the interface prefix");
    }

    [TestMethod]
    public void PlainModelGetsTheInterfacePrefixOfTheLanguage()
    {
        Options.GetGlobal<GeneratorOptions>().Language = new CsharpLanguage(this.resolver);
        this.resolver.Get<ReflectionModelReader>().Read(typeof(Detail));
        this.resolver.Create<ModelWriter>().FormatNames().Write("Output");

        Assert.AreEqual("I" + nameof(Detail), this.Get(nameof(Detail)).Name);
    }

    [TestMethod]
    public void PreferClassesAppliesToTheTypeAndEverythingGeneratedThroughIt()
    {
        this.Write<PreferClassesModel>();

        Assert.IsFalse(this.Get(nameof(PreferClassesModel)).IsInterface);
        Assert.IsFalse(this.Get(nameof(Detail)).IsInterface, "The preference is passed on to the referenced types");
    }

    [TestMethod]
    public void PreferInterfacesWinsOverClassesPreferredAbove()
    {
        GeneratorOptions parent = new(null, null, "global") { PreferInterfaces = false };
        GeneratorOptions options = (GeneratorOptions)new GeneratorOptionsFactory().CreateGlobal(typeof(GeneratorOptions), typeof(PreferInterfacesModel), parent);

        Assert.IsTrue(options.PreferInterfaces);
    }

    [TestMethod]
    public void ForceClassAppliesToTheTypeOnly()
    {
        this.Write<ForceClassModel>();

        ClassTemplate model = this.Get(nameof(ForceClassModel));
        Assert.IsFalse(model.IsInterface);
        Assert.AreEqual(nameof(ForceClassModel), model.Name, "A class must not get the interface prefix");
        Assert.IsTrue(this.Get(nameof(Detail)).IsInterface, "The force is not passed on to the referenced types");
    }

    [TestMethod]
    public void ForceInterfaceWinsOverPreferClassesOfTheReferencingType()
    {
        this.Write<PreferClassesWithForcedInterface>();

        Assert.IsFalse(this.Get(nameof(PreferClassesWithForcedInterface)).IsInterface);
        Assert.IsTrue(this.Get(nameof(ForceInterfaceDetail)).IsInterface);
    }

    [TestMethod]
    public void ForceIsKeptBesideAnotherGenerateClass()
    {
        this.Write<RenamedForceClassModel>();

        Assert.IsFalse(this.Get("RenamedForceClass").IsInterface);
    }

    [TestMethod]
    public void FluentForceClassOnATypeWinsOverPreferInterfaces()
    {
        new SetFluentSyntax(typeof(PlainModel), this.resolver.Get<Options>()).ForceClass();

        this.Write<PlainModel>();

        Assert.IsFalse(this.Get(nameof(PlainModel)).IsInterface);
    }

    [TestMethod]
    public void FluentForceOnAnAssemblyFails()
    {
        SetFluentSyntax syntax = new(typeof(InterfaceOrClassTests).Assembly, this.resolver.Get<Options>());

        Assert.ThrowsException<InvalidOperationException>(() => syntax.ForceClass());
        Assert.ThrowsException<InvalidOperationException>(() => syntax.ForceInterface());
    }

    [TestMethod]
    public void ForceClassAndForceInterfaceTogetherFail()
    {
        this.AssertFails<ForceBothModel>("at the same time");
    }

    [TestMethod]
    public void FluentForceTogetherWithTheOtherAttributeFails()
    {
        new SetFluentSyntax(typeof(ForceInterfaceDetail), this.resolver.Get<Options>()).ForceClass();

        this.AssertFails<ForceInterfaceDetail>("at the same time");
    }

    [TestMethod]
    public void ForceInterfaceWithConstantsFails()
    {
        this.AssertFails<ForceInterfaceWithConstant>("has constants");
    }

    [TestMethod]
    public void ForceClassOnAnInterfaceFails()
    {
        this.AssertFails<IForceClassInterface>("is an interface");
    }

    [TestMethod]
    public void ForceOnAnEnumFails()
    {
        this.AssertFails<ForceClassEnum>("is an enum");
    }

    private void Write<T>()
    {
        this.resolver.Get<ReflectionModelReader>().Read(typeof(T));
        this.resolver.Create<TypeScriptModelWriter>().FormatNames().Write("Output");
    }

    private void AssertFails<T>(string expectedMessagePart)
    {
        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(this.Write<T>);
        StringAssert.Contains(exception.Message, expectedMessagePart);
    }

    private ClassTemplate Get(string name)
    {
        List<ClassTemplate> classes = this.files.SelectMany(file => file.Namespaces)
                                          .SelectMany(ns => ns.Children)
                                          .OfType<ClassTemplate>()
                                          .ToList();
        return classes.FirstOrDefault(x => x.Name == name || x.Name == "I" + name)
               ?? throw new AssertFailedException($"{name} was not written, found {string.Join(", ", classes.Select(x => x.Name))}");
    }
}
