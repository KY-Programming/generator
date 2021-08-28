using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Common.Tests.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileWriter = KY.Generator.Output.FileWriter;

namespace KY.Generator.Common.Tests
{
    [TestClass]
    public class TemplateWriterTests : Codeable
    {
        private IDependencyResolver resolver;
        private IOutputCache output;
        private IOptions options;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.output = new FileWriter(this.options);
            this.options.Language = new TestLanguage(this.resolver);
            this.options = new OptionsSet(null, null);
        }

        [TestMethod]
        public void AccessIndexWriter()
        {
            AccessIndexWriter writer = new AccessIndexWriter();
            writer.Write(new AccessIndexTemplate(Code.Local("test")), this.output);
            Assert.AreEqual("[test]", this.output.ToString());
        }

        [TestMethod]
        public void AppendStringWriter()
        {
            AppendStringWriter writer = new AppendStringWriter();
            writer.Write(new AppendStringTemplate(Code.Local("test")), this.output);
            Assert.AreEqual(" + test", this.output.ToString());
        }

        [TestMethod]
        public void AssignWriter()
        {
            AssignWriter writer = new AssignWriter();
            writer.Write(new AssignTemplate(Code.Local("test")), this.output);
            Assert.AreEqual("= test", this.output.ToString());
        }

        [TestMethod]
        public void AsWriter()
        {
            AsWriter writer = new AsWriter();
            writer.Write(new AsTemplate(Code.Type("test")), this.output);
            Assert.AreEqual("as test", this.output.ToString());
        }

        [TestMethod]
        public void BlankLineWriter()
        {
            BlankLineWriter writer = new BlankLineWriter();
            writer.Write(new BlankLineTemplate(), this.output);
            Assert.AreEqual("", this.output.ToString());
        }

        [TestMethod]
        public void CaseWriter()
        {
            CaseWriter writer = new CaseWriter();
            CaseTemplate template = new CaseTemplate(Code.String("test"));
            template.Code.AddLine(Code.Comment("code here"));
            writer.Write(template, this.output);
            Assert.AreEqual("case \"test\":\r\n    // code here\r\n    break;", this.output.ToString());
        }

        [TestMethod]
        public void ClassGenericWriter()
        {
            Assert.Inconclusive("Not implemented yet");
            ClassGenericWriter writer = new ClassGenericWriter();
            writer.Write(new ClassGenericTemplate("test"), this.output);
            Assert.AreEqual("test", this.output.ToString());
        }

        [TestMethod]
        public void ClassWriter()
        {
            ClassWriter writer = new ClassWriter(this.options);
            writer.Write(new ClassTemplate((NamespaceTemplate)null, "test"), this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassOneProperty()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddProperty("Prop1", Code.Type("string"));
            ClassWriter writer = new ClassWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n    public string Prop1 { get; set; }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassOnePropertyAndOneMethod()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.AddProperty("Prop1", Code.Type("string"));
            template.AddMethod("Meth1", Code.Type("string"));
            ClassWriter writer = new ClassWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("public partial class test\r\n{\r\n    public string Prop1 { get; set; }\r\n\r\n    public string Meth1()\r\n    {\r\n    }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ClassWithComment()
        {
            ClassTemplate template = new ClassTemplate((NamespaceTemplate)null, "test");
            template.Comment = Code.Comment("test comment");
            ClassWriter writer = new ClassWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("// test comment\r\npublic partial class test\r\n{\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void CommentWriter()
        {
            CommentWriter writer = new CommentWriter();
            writer.Write(new CommentTemplate("first line"), this.output);
            Assert.AreEqual("// first line", this.output.ToString());
        }

        [TestMethod]
        public void MultilineComment()
        {
            CommentWriter writer = new CommentWriter();
            writer.Write(new CommentTemplate("first line\r\nsecond line\r\nthird line"), this.output);
            Assert.AreEqual("// first line\r\n// second line\r\n// third line", this.output.ToString());
        }

        [TestMethod]
        public void ElseIfWriter()
        {
            ElseIfTemplate template = new ElseIfTemplate(null, Code.Local("variable"));
            template.Code.AddLine(Code.Comment("Some code here"));
            ElseIfWriter writer = new ElseIfWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("else if (variable)\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ElseWriter()
        {
            ElseTemplate template = new ElseTemplate(null);
            template.Code.AddLine(Code.Comment("Some code here"));
            ElseWriter writer = new ElseWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("else\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void EnumWriter()
        {
            EnumTemplate template = new EnumTemplate((NamespaceTemplate)null, "test");
            template.Values.Add(new EnumValueTemplate("value", Code.Number(0)));
            EnumWriter writer = new EnumWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("public enum test\r\n{\r\n    value = 0\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ExecuteFieldWriter()
        {
            ExecuteFieldWriter writer = new ExecuteFieldWriter();
            writer.Write(new ExecuteFieldTemplate("field"), this.output);
            Assert.AreEqual("field", this.output.ToString());
        }

        [TestMethod]
        public void ExecuteGenericMethodWriter()
        {
            ExecuteGenericMethodWriter writer = new ExecuteGenericMethodWriter();
            writer.Write(new ExecuteGenericMethodTemplate("test", Code.Type("type").Yield(), Code.Local("parameter")), this.output);
            Assert.AreEqual("test<type>(parameter)", this.output.ToString());
        }

        [TestMethod]
        public void ExecuteMethodWriter()
        {
            ExecuteMethodWriter writer = new ExecuteMethodWriter();
            writer.Write(new ExecuteMethodTemplate("test", Code.Local("parameter1"), Code.Local("parameter2")), this.output);
            Assert.AreEqual("test(parameter1, parameter2)", this.output.ToString());
        }

        [TestMethod]
        public void ExecutePropertyWriter()
        {
            ExecutePropertyWriter writer = new ExecutePropertyWriter();
            writer.Write(new ExecutePropertyTemplate("test"), this.output);
            Assert.AreEqual("test", this.output.ToString());
        }

        [TestMethod]
        public void FieldWriter()
        {
            FieldWriter writer = new();
            writer.Write(new FieldTemplate(null, "test", Code.Type("type")), this.output);
            Assert.AreEqual("private type test;", this.output.ToString());
        }

        [TestMethod]
        public void FieldDefaultValue()
        {
            FieldTemplate template = new FieldTemplate(null, "test", Code.Type("type"));
            template.DefaultValue = Code.String("default");
            FieldWriter writer = new();
            writer.Write(template, this.output);
            Assert.AreEqual("private type test = \"default\";", this.output.ToString());
        }

        [TestMethod]
        public void IfWriter()
        {
            Assert.Inconclusive("Not implemented yet");
            IfTemplate template = new IfTemplate(Code.Local("test").Equals().String("string"));
            template.Code.AddLine(Code.Comment("Some code here"));
            IfWriter writer = new IfWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("if (test == \"string\")\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void InlineIfWriter()
        {
            InlineIfTemplate template = new InlineIfTemplate(Code.Local("test"), Code.String("true"), Code.String("false"));
            InlineIfWriter writer = new InlineIfWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("test ? \"true\" : \"false\"", this.output.ToString());
        }

        [TestMethod]
        public void LambdaWriter()
        {
            LambdaTemplate template = new LambdaTemplate("parameter".Yield(), Code.Local("parameter").Method("test"));
            LambdaWriter writer = new LambdaWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("parameter => parameter.test()", this.output.ToString());
        }

        [TestMethod]
        public void LambdaMultiline()
        {
            LambdaTemplate template = new LambdaTemplate("parameter".Yield(), Code.Multiline().AddLine(Code.Local("parameter").Method("test").Close()));
            LambdaWriter writer = new LambdaWriter(this.options);
            writer.Write(template, this.output);
            Assert.AreEqual("parameter =>\r\n{\r\n    parameter.test();\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void LocalVariableWriter()
        {
            LocalVariableWriter writer = new LocalVariableWriter();
            writer.Write(new LocalVariableTemplate("test"), this.output);
            Assert.AreEqual("test", this.output.ToString());
        }

        [TestMethod]
        public void MethodWriter()
        {
            MethodTemplate template = new MethodTemplate(null, "test", Code.Type("type"));
            template.Code.AddLine(Code.Comment("Some code here"));
            MethodWriter writer = new MethodWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public type test()\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void NullWriter()
        {
            NullWriter writer = new NullWriter();
            writer.Write(new NullTemplate(), this.output);
            Assert.AreEqual("null", this.output.ToString());
        }

        [TestMethod]
        public void StaticMethod()
        {
            MethodTemplate template = new MethodTemplate(null, "test", Code.Type("type")).Static();
            template.Code.AddLine(Code.Comment("Some code here"));
            MethodWriter writer = new MethodWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public static type test()\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void OverrideMethod()
        {
            MethodTemplate template = new MethodTemplate(null, "test", Code.Type("type")).Override();
            template.Code.AddLine(Code.Comment("Some code here"));
            MethodWriter writer = new MethodWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public override type test()\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ParameterMethod()
        {
            Assert.Inconclusive("Not implemented yet");
            MethodTemplate template = new MethodTemplate(null, "test", Code.Type("type"));
            template.Parameters.Add(new ParameterTemplate(Code.Type("other"), "param"));
            template.Code.AddLine(Code.Comment("Some code here"));
            MethodWriter writer = new MethodWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public type test(other param)\r\n{\r\n    // Some code here\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void PropertyWithEmptyComment()
        {
            PropertyTemplate template = new PropertyTemplate(null, "Property", Code.Type("string"))
                .WithComment("");
            this.output.Add(template);
            Assert.AreEqual("public string Property { get; set; }", this.output.ToString());
        }

        [TestMethod]
        public void NamespaceWriter()
        {
            NamespaceTemplate template = new NamespaceTemplate(null, "test");
            template.AddClass("testClass");
            NamespaceWriter writer = new();
            writer.Write(template, this.output);
            Assert.AreEqual("namespace test\r\n{\r\n    public partial class testClass\r\n    {\r\n    }\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void NewWriter()
        {
            NewWriter writer = new NewWriter();
            writer.Write(new NewTemplate(Code.Type("type")), this.output);
            Assert.AreEqual("new type()", this.output.ToString());
        }

        [TestMethod]
        public void NewWithParameter()
        {
            NewWriter writer = new NewWriter();
            writer.Write(new NewTemplate(Code.Type("type"), Code.Local("parameter")), this.output);
            Assert.AreEqual("new type(parameter)", this.output.ToString());
        }

        [TestMethod]
        public void NotWriter()
        {
            NotWriter writer = new NotWriter();
            writer.Write(new NotTemplate(), this.output);
            Assert.AreEqual("!", this.output.ToString());
        }

        [TestMethod]
        public void NullValueWriter()
        {
            NullValueWriter writer = new NullValueWriter();
            writer.Write(new NullValueTemplate(), this.output);
            Assert.AreEqual("null", this.output.ToString());
        }

        [TestMethod]
        public void NumberWriter()
        {
            NumberWriter writer = new NumberWriter();
            writer.Write(new NumberTemplate(1), this.output);
            Assert.AreEqual("1", this.output.ToString());
        }

        [TestMethod]
        public void OperatorWriter()
        {
            OperatorWriter writer = new OperatorWriter();
            writer.Write(new OperatorTemplate(Operator.And), this.output);
            Assert.AreEqual("&&", this.output.ToString());
        }

        [TestMethod]
        public void PropertyWriter()
        {
            PropertyWriter writer = new PropertyWriter();
            writer.Write(new PropertyTemplate(null, "test", Code.Type("type")), this.output);
            Assert.AreEqual("public type test { get; set; }", this.output.ToString());
        }

        [TestMethod]
        public void PropertyDefaultValue()
        {
            PropertyTemplate template = new PropertyTemplate(null, "test", Code.Type("type"));
            template.DefaultValue = Code.String("value");
            PropertyWriter writer = new PropertyWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("public type test { get; set; } = \"value\";", this.output.ToString());
        }

        [TestMethod]
        public void ReturnWriter()
        {
            ReturnWriter writer = new ReturnWriter();
            writer.Write(new ReturnTemplate(Code.String("string")), this.output);
            Assert.AreEqual("return \"string\";", this.output.ToString());
        }

        [TestMethod]
        public void StringWriter()
        {
            StringWriter writer = new StringWriter(this.options);
            writer.Write(new StringTemplate("string"), this.output);
            Assert.AreEqual("\"string\"", this.output.ToString());
        }

        [TestMethod]
        public void SwitchWriter()
        {
            SwitchTemplate template = new SwitchTemplate(Code.Local("value"));
            CaseTemplate caseTemplate = new CaseTemplate(Code.String("one"));
            caseTemplate.Code.AddLine(Code.Comment("Some code here"));
            template.Cases.Add(caseTemplate);
            template.Default.AddLine(Code.Comment("Some code here"));
            SwitchWriter writer = new SwitchWriter();
            writer.Write(template, this.output);
            Assert.AreEqual("switch (value)\r\n{\r\n    case \"one\":\r\n        // Some code here\r\n        break;\r\n    default:\r\n        // Some code here\r\n        break;\r\n}", this.output.ToString());
        }

        [TestMethod]
        public void ThisWriter()
        {
            ThisWriter writer = new ThisWriter();
            writer.Write(new ThisTemplate(), this.output);
            Assert.AreEqual("this", this.output.ToString());
        }

        [TestMethod]
        public void TypeOfWriter()
        {
            TypeOfWriter writer = new TypeOfWriter();
            writer.Write(new TypeOfTemplate(Code.Type("test")), this.output);
            Assert.AreEqual("typeof(test)", this.output.ToString());
        }

        [TestMethod]
        public void VoidWriter()
        {
            VoidWriter writer = new VoidWriter();
            writer.Write(new VoidTemplate(), this.output);
            Assert.AreEqual("void", this.output.ToString());
        }
    }
}
