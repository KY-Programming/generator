using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

[TestClass]
public class GenerateClassAttributeTests
{
    [GenerateClass(Replace = "Dto")]
    private class ReplacedClass
    { }

    [GenerateClass(Replace = "Dto", With = "Model")]
    private class ReplacedWithClass
    { }

    [GenerateClass(Replace = "Dto")]
    [GenerateClass(Replace = "Transfer")]
    private class MultipleReplacedClass
    { }

    [GenerateClass(Name = "Account")]
    private class RenamedClass
    { }

    private class PlainClass
    { }

    private static GeneratorOptions Read<T>()
    {
        return (GeneratorOptions)new GeneratorOptionsFactory().CreateGlobal(typeof(GeneratorOptions), typeof(T), null);
    }

    [TestMethod]
    public void ReplaceWithoutWithRemovesThePart()
    {
        GeneratorOptions options = Read<ReplacedClass>();

        Assert.AreEqual(string.Empty, options.ReplaceName["Dto"]);
        Assert.IsNull(options.Rename);
    }

    [TestMethod]
    public void ReplaceWithReplacesThePart()
    {
        GeneratorOptions options = Read<ReplacedWithClass>();

        Assert.AreEqual("Model", options.ReplaceName["Dto"]);
    }

    [TestMethod]
    public void MultipleAttributesAreCombined()
    {
        GeneratorOptions options = Read<MultipleReplacedClass>();

        Assert.AreEqual(2, options.ReplaceName.Count);
        Assert.AreEqual(string.Empty, options.ReplaceName["Dto"]);
        Assert.AreEqual(string.Empty, options.ReplaceName["Transfer"]);
    }

    [TestMethod]
    public void NameSetsRename()
    {
        GeneratorOptions options = Read<RenamedClass>();

        Assert.AreEqual("Account", options.Rename);
        Assert.AreEqual(0, options.ReplaceName.Count);
    }

    [TestMethod]
    public void ClassWithoutAttributeStaysUntouched()
    {
        GeneratorOptions options = Read<PlainClass>();

        Assert.IsNull(options.Rename);
        Assert.AreEqual(0, options.ReplaceName.Count);
    }

    [TestMethod]
    public void MemberAttributeStillReadsNameAndReplace()
    {
        GeneratorOptions options = (GeneratorOptions)new GeneratorOptionsFactory()
                                                    .CreateGlobal(typeof(GeneratorOptions), typeof(MemberHolder).GetProperty(nameof(MemberHolder.Renamed))!, null);

        Assert.AreEqual("renamedProperty", options.Rename);
        Assert.AreEqual("Nice", options.ReplaceName["Ugly"]);
        Assert.AreEqual(typeof(string), options.ReturnType?.Type);
    }

    private class MemberHolder
    {
        [GenerateProperty(Name = "renamedProperty", Replace = "Ugly", With = "Nice", Type = typeof(string))]
        public int Renamed { get; set; }
    }
}
