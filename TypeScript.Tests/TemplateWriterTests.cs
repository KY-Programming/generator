using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileWriter = KY.Generator.Output.FileWriter;

namespace KY.Generator.TypeScript.Tests;

[TestClass]
public class TemplateWriterTests : Codeable
{
    private IDependencyResolver resolver;
    private IOutputCache output;
    private Options options;

    [TestInitialize]
    public void Initialize()
    {
        this.resolver = new DependencyResolver();
        this.options = new Options();
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        generatorOptions.Language = new TypeScriptLanguage(this.resolver);
        this.output = new FileWriter(generatorOptions);
    }

    [TestMethod]
    public void ClassOneFieldAndOneConstructor()
    {
        ClassTemplate template = new((NamespaceTemplate)null, "test");
        template.AddField("field1", Code.Type("string"));
        template.AddConstructor();
        ClassWriter writer = new(this.options);
        writer.Write(template, this.output);
        Assert.AreEqual("export class test {\r\n    private field1: string;\r\n\r\n    public constructor() {\r\n    }\r\n}", this.output.ToString());
    }
}
