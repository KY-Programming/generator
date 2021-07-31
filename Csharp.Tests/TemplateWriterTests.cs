using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Csharp.Templates;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommentWriter = KY.Generator.Csharp.Writers.CommentWriter;

namespace KY.Generator.Csharp.Tests
{
    [TestClass]
    public class TemplateWriterTests : Codeable
    {
        private IDependencyResolver resolver;
        private IOutputCache output;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.output = new FileWriter(CsharpLanguage.Instance);
        }

        [TestMethod]
        public void AttributeWriter()
        {
            AttributeWriter writer = new AttributeWriter(this.output.Language.CastTo<BaseLanguage>());
            writer.Write(new AttributeTemplate("test"), this.output);
            Assert.AreEqual("[test]", this.output.ToString());
        }

        [TestMethod]
        public void AttributeWithStringValue()
        {
            AttributeWriter writer = new AttributeWriter(this.output.Language.CastTo<BaseLanguage>());
            writer.Write(new AttributeTemplate("test", Code.String("value")), this.output);
            Assert.AreEqual("[test(\"value\")]", this.output.ToString());
        }

        [TestMethod]
        public void AttributeWithProperty()
        {
            AttributeTemplate template = new AttributeTemplate("test");
            template.Properties.Add("key", "value");
            AttributeWriter writer = new AttributeWriter(this.output.Language.CastTo<BaseLanguage>());
            writer.Write(template, this.output);
            Assert.AreEqual("[test(key = \"value\")]", this.output.ToString());
        }

        [TestMethod]
        public void AttributeOnProperty()
        {
            PropertyTemplate template = new PropertyTemplate(null, "Property", Code.Type("string"))
                .WithAttribute("Attribute", Code.String("value"));
            this.output.Language.Write(template, this.output);
            Assert.AreEqual("[Attribute(\"value\")]\r\npublic string Property { get; set; }", this.output.ToString());
        }

        [TestMethod]
        public void AttributeOnMultiplePropertiesInClass()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "MyClass");
            template.AddProperty("Property1", Code.Type("string"))
                    .WithAttribute("Attribute", Code.String("value"));
            template.AddProperty("Property2", Code.Type("string"))
                    .WithAttribute("Attribute", Code.String("value"));
            this.output.Language.Write(template, this.output);
            Assert.AreEqual("public partial class MyClass\r\n{\r\n    [Attribute(\"value\")]\r\n    public string Property1 { get; set; }\r\n\r\n    [Attribute(\"value\")]\r\n    public string Property2 { get; set; }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void BaseTypeWriter()
        {
            BaseTypeWriter writer = new BaseTypeWriter();
            writer.Write(new BaseTypeTemplate(new ClassTemplate((ClassTemplate)null, "test"), Code.Type("type")), this.output);
            Assert.AreEqual(" : type", this.output.ToString());
        }

        [TestMethod]
        public void CastWriter()
        {
            CastWriter writer = new CastWriter();
            writer.Write(new CastTemplate(Code.Type("type")).Local("variable"), this.output);
            Assert.AreEqual("(type)variable", this.output.ToString());
        }

        [TestMethod]
        public void CommentWriter()
        {
            CommentWriter writer = new CommentWriter();
            writer.Write(new CommentTemplate("Summary here", CommentType.Summary), this.output);
            Assert.AreEqual("/// <summary>\r\n/// Summary here\r\n/// </summary>", this.output.ToString());
        }

        [TestMethod]
        public void ConstraintWriter()
        {
            ConstraintWriter writer = new ConstraintWriter();
            writer.Write(new ConstraintTemplate("T", new List<TypeTemplate> { Code.Type("type") }), this.output);
            Assert.AreEqual("\r\n    where T : type", this.output.ToString());
        }

        [TestMethod]
        public void ConstructorWriter()
        {
            ConstructorWriter writer = new ConstructorWriter();
            ClassTemplate classTemplate = new ClassTemplate((NamespaceTemplate)null, "Test");
            writer.Write(new ConstructorTemplate(classTemplate), this.output);
            Assert.AreEqual("public Test()\r\n{\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ConstructorWithBaseCall()
        {
            ConstructorWriter writer = new ConstructorWriter();
            ClassTemplate classTemplate = new ClassTemplate((NamespaceTemplate)null, "Test");
            writer.Write(new ConstructorTemplate(classTemplate).WithBaseConstructor(), this.output);
            Assert.AreEqual("public Test()\r\n    : base()\r\n{\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ConstructorWithThisCall()
        {
            ConstructorWriter writer = new ConstructorWriter();
            ClassTemplate classTemplate = new ClassTemplate((NamespaceTemplate)null, "Test");
            writer.Write(new ConstructorTemplate(classTemplate).WithThisConstructor(), this.output);
            Assert.AreEqual("public Test()\r\n    : this()\r\n{\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassOnePropertyAndOneConstructor()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddProperty("Prop1", Code.Type("string"));
            template.AddConstructor();
            ClassWriter writer = new ClassWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n    public string Prop1 { get; set; }\r\n\r\n    public test()\r\n    {\r\n    }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassOneNormalPropertyAndOnePropertyWithAttribute()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddProperty("Prop1", Code.Type("string"));
            template.AddProperty("Prop2", Code.Type("string")).WithAttribute("attr");
            ClassWriter writer = new ClassWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n    public string Prop1 { get; set; }\r\n\r\n    [attr]\r\n    public string Prop2 { get; set; }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassOnePropertyWithAttributeAndOneNormalProperty()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddProperty("Prop1", Code.Type("string")).WithAttribute("attr");
            template.AddProperty("Prop2", Code.Type("string"));
            ClassWriter writer = new ClassWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n    [attr]\r\n    public string Prop1 { get; set; }\r\n\r\n    public string Prop2 { get; set; }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void DeclareWriter()
        {
            DeclareWriter writer = new DeclareWriter();
            writer.Write(new DeclareTemplate(Code.Type("type"), "variable", Code.String("string")), this.output);
            Assert.AreEqual("type variable = \"string\";", this.output.ToString());
        }

        [TestMethod]
        public void ParameterWriter()
        {
            ParameterWriter writer = new ParameterWriter();
            writer.Write(new ParameterTemplate(Code.Type("type"), "parameter"), this.output);
            Assert.AreEqual("type parameter", this.output.ToString());
        }

        [TestMethod]
        public void ParameterWithDefaultValue()
        {
            ParameterWriter writer = new ParameterWriter();
            writer.Write(new ParameterTemplate(Code.Type("type"), "parameter", Code.String("default")), this.output);
            Assert.AreEqual("type parameter = \"default\"", this.output.ToString());
        }

        [TestMethod]
        public void ParameterWithAttribute()
        {
            ParameterWriter writer = new ParameterWriter();
            writer.Write(new ParameterTemplate(Code.Type("Type"), "parameter").WithAttribute("Attr"), this.output);
            Assert.AreEqual("[Attr] Type parameter", this.output.ToString());
        }

        [TestMethod]
        public void ThrowWriter()
        {
            ThrowWriter writer = new ThrowWriter();
            writer.Write(new ThrowTemplate(Code.Type("type")), this.output);
            Assert.AreEqual("throw new type();", this.output.ToString());
        }

        [TestMethod]
        public void ThrowWithParameter()
        {
            ThrowWriter writer = new ThrowWriter();
            writer.Write(new ThrowTemplate(Code.Type("type"), Code.Local("parameter")), this.output);
            Assert.AreEqual("throw new type(parameter);", this.output.ToString());
        }

        [TestMethod]
        public void UsingWriter()
        {
            UsingWriter writer = new UsingWriter();
            writer.Write(new UsingTemplate("test", null, null), this.output);
            Assert.AreEqual("using test;", this.output.ToString());
        }
    }
}
