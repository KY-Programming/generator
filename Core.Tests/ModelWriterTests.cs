using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Core.Tests.Models;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Core.Tests
{
    [TestClass]
    public class ModelWriterTests
    {
        private IDependencyResolver resolver;
        private ModelWriter writer;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.writer = this.resolver.Create<ModelWriter>();
        }

        [TestMethod]
        public void File()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Name = "Test1";
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            Assert.AreEqual(1, files.Count, "Unexpected number of files");
            Assert.AreEqual("Test1", files[0].Name, "Unexpected file name");
        }

        [TestMethod]
        public void Namespace()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Namespace = "KY.Test";
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            Assert.AreEqual(1, files[0].Namespaces.Count, "Unexpected number of namespace");
            Assert.AreEqual("KY.Test", files[0].Namespaces[0].Name, "Unexpected namespace");
        }

        [TestMethod]
        public void ClassName()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Name = "Test1";
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            Assert.AreEqual("Test1", files[0].Namespaces[0].Children[0].Name, "Unexpected class name");
        }

        [TestMethod]
        public void OneField()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Name = "Test1";
            model.Namespace = "KY.Test";
            model.Fields.Add(new FieldTransferObject { Name = "Field1", Type = new TypeTransferObject { Name = "string" } });
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
            Assert.AreEqual(1, classTemplate.Fields.Count, "Unexpected number of fields");
            Assert.AreEqual("Field1", classTemplate.Fields[0].Name, "Unexpected field name");
            Assert.AreEqual("string", classTemplate.Fields[0].Type.Name, "Unexpected field type");
            Assert.AreEqual(0, classTemplate.Properties.Count, "Unexpected number of properties");
        }

        [TestMethod]
        public void OneProperty()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Name = "Test1";
            model.Namespace = "KY.Test";
            model.Properties.Add(new PropertyTransferObject { Name = "Prop1", Type = new TypeTransferObject { Name = "string" } });
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
            Assert.AreEqual(1, classTemplate.Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("Prop1", classTemplate.Properties[0].Name, "Unexpected property name");
            Assert.AreEqual("string", classTemplate.Properties[0].Type.Name, "Unexpected property type");
            Assert.AreEqual(0, classTemplate.Fields.Count, "Unexpected number of fields");
        }

        [TestMethod]
        public void OnePropertyWithDifferentNamespace()
        {
            TestConfiguration configuration = new TestConfiguration();
            List<ModelTransferObject> transferObjects = new List<ModelTransferObject>();
            ModelTransferObject model = new ModelTransferObject();
            model.Name = "Test1";
            model.Namespace = "KY.Test";
            model.Properties.Add(new PropertyTransferObject { Name = "Prop1", Type = new TypeTransferObject { Name = "Test2", Namespace = "KY.Test.Different" } });
            transferObjects.Add(model);
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
            Assert.AreEqual(1, classTemplate.Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("Prop1", classTemplate.Properties[0].Name, "Unexpected property name");
            Assert.AreEqual("Test2", classTemplate.Properties[0].Type.Name, "Unexpected property type");
            Assert.AreEqual(1, classTemplate.Usings.Count, "Unexpected number of usings");
            Assert.AreEqual("KY.Test.Different", classTemplate.Usings[0].Namespace, "Unexpected property namespace");
            Assert.AreEqual(0, classTemplate.Fields.Count, "Unexpected number of fields");
        }
    }
}