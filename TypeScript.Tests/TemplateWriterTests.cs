using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.TypeScript.Tests
{
    [TestClass]
    public class TemplateWriterTests : MsTestBase
    {
        private IDependencyResolver resolver;
        private IOutputCache output;
        private static readonly Code Code = default;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.output = new FileWriter(TypeScriptLanguage.Instance);
        }


        [TestMethod]
        public void ClassOneFieldAndOneConstructor()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddField("field1", Code.Type("string"));
            template.AddConstructor();
            ClassWriter writer = new ClassWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("export class test {\r\n    private field1: string;\r\n\r\n    public constructor() {\r\n    }\r\n}", this.output.ToString());
        }
    }
}
