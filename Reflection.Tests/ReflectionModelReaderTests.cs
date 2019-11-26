using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Reflection.Tests
{
    [TestClass]
    public class ReflectionModelReaderTests
    {
        private ReflectionModelReader reader;

        [TestInitialize]
        public void Initialize()
        {
            this.reader = new ReflectionModelReader();
        }

        [TestMethod]
        public void OneField()
        {
            List<ModelTransferObject> objects = this.Read(typeof(OneField));
            Assert.AreEqual(1, objects.Count, "Unexpected number of models");
            Assert.AreEqual(1, objects[0].Fields.Count, "Unexpected number of fields");
            Assert.AreEqual(0, objects[0].Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("Field1", objects[0].Fields[0].Name, "Unexpected field name");
            Assert.AreEqual("String", objects[0].Fields[0].Type.Name, "Unexpected field type");
        }

        [TestMethod]
        public void OneProperty()
        {
            List<ModelTransferObject> objects = this.Read(typeof(OneProperty));
            Assert.AreEqual(1, objects.Count, "Unexpected number of models");
            Assert.AreEqual(0, objects[0].Fields.Count, "Unexpected number of fields");
            Assert.AreEqual(1, objects[0].Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("Prop1", objects[0].Properties[0].Name, "Unexpected property name");
            Assert.AreEqual("String", objects[0].Properties[0].Type.Name, "Unexpected property type");
        }

        [TestMethod]
        public void OnePropertyWithCustomType()
        {
            List<ModelTransferObject> objects = this.Read(typeof(OnePropertyWithCustomType));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual(0, objects[0].Fields.Count, "Unexpected number of fields");
            Assert.AreEqual(1, objects[0].Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("Prop2", objects[0].Properties[0].Name, "Unexpected property name");
            Assert.AreEqual("OneProperty", objects[0].Properties[0].Type.Name, "Unexpected property type");
            Assert.AreEqual("Prop1", objects[1].Properties[0].Name, "Unexpected property name");
            Assert.AreEqual("String", objects[1].Properties[0].Type.Name, "Unexpected property type");
        }

        [TestMethod]
        public void Recursive()
        {
            List<ModelTransferObject> objects = this.Read(typeof(RecursiveType));
            Assert.AreEqual(1, objects.Count, "Unexpected number of models");
            Assert.AreEqual(0, objects[0].Fields.Count, "Unexpected number of fields");
            Assert.AreEqual(1, objects[0].Properties.Count, "Unexpected number of properties");
        }

        [TestMethod]
        public void CustomTypeInArray()
        {
            List<ModelTransferObject> objects = this.Read(typeof(CustomTypeInArray));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual("CustomTypeInArray", objects[0].Name);
            Assert.AreEqual("SubType", objects[1].Name);
        }

        [TestMethod]
        public void CustomTypeInList()
        {
            List<ModelTransferObject> objects = this.Read(typeof(CustomTypeInList));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual("CustomTypeInList", objects[0].Name);
            Assert.AreEqual("SubType", objects[1].Name);
        }

        [TestMethod]
        public void BasedOnStringList()
        {
            List<ModelTransferObject> objects = this.Read(typeof(BasedOnStringList));
            Assert.AreEqual(1, objects.Count, "Unexpected number of models");
            Assert.AreEqual("BasedOnStringList", objects[0].Name);
            Assert.AreEqual("List", objects[0].BasedOn.Name);
            Assert.AreEqual(1, objects[0].BasedOn.Generics.Count, "Unexpected number of generics");
            Assert.AreEqual("String", objects[0].BasedOn.Generics[0].Type.Name);
        }

        [TestMethod]
        public void BasedOnSubtypeList()
        {
            List<ModelTransferObject> objects = this.Read(typeof(BasedOnSubtypeList));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual("BasedOnSubtypeList", objects[0].Name);
            Assert.AreEqual("List", objects[0].BasedOn.Name);
            Assert.AreEqual(1, objects[0].BasedOn.Generics.Count, "Unexpected number of generics");
            Assert.AreEqual("SubType", objects[0].BasedOn.Generics[0].Type.Name);
            Assert.AreEqual("SubType", objects[1].Name);
        }

        [TestMethod]
        public void BasedOnCustomGenericString()
        {
            List<ModelTransferObject> objects = this.Read(typeof(BasedOnCustomGenericString));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual("BasedOnCustomGenericString", objects[0].Name);
            Assert.AreEqual("CustomGeneric", objects[0].BasedOn.Name);
            Assert.AreEqual("CustomGeneric", objects[1].Name);
            Assert.AreEqual(1, objects[0].BasedOn.Generics.Count, "Unexpected number of generics");
        }

        [TestMethod]
        public void BasedOnCustomGenericSubtype()
        {
            List<ModelTransferObject> objects = this.Read(typeof(BasedOnCustomGenericSubtype));
            Assert.AreEqual(3, objects.Count, "Unexpected number of models");
            Assert.AreEqual("BasedOnCustomGenericSubtype", objects[0].Name);
            Assert.AreEqual("CustomGeneric", objects[0].BasedOn.Name);
            Assert.AreEqual(1, objects[0].BasedOn.Generics.Count, "Unexpected number of generics");
            Assert.AreEqual("CustomGeneric", objects[1].Name);
            Assert.AreEqual("SubType", objects[2].Name);
        }

        [TestMethod]
        public void CustomGenericStringProperty()
        {
            List<ModelTransferObject> objects = this.Read(typeof(CustomGenericStringProperty));
            Assert.AreEqual(2, objects.Count, "Unexpected number of models");
            Assert.AreEqual("CustomGenericStringProperty", objects[0].Name);
            Assert.AreEqual(1, objects[0].Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("CustomGeneric", objects[0].Properties[0].Type.Name);
            Assert.AreEqual(1, objects[0].Properties[0].Type.Generics.Count, "Unexpected number of generics");
            Assert.AreEqual("CustomGeneric", objects[1].Name);
        }

        [TestMethod]
        public void CustomGenericSubtypeProperty()
        {
            List<ModelTransferObject> objects = this.Read(typeof(CustomGenericSubtypeProperty));
            Assert.AreEqual(3, objects.Count, "Unexpected number of models");
            Assert.AreEqual("CustomGenericSubtypeProperty", objects[0].Name);
            Assert.AreEqual(1, objects[0].Properties.Count, "Unexpected number of properties");
            Assert.AreEqual("CustomGeneric", objects[0].Properties[0].Type.Name);
            Assert.AreEqual(1, objects[0].Properties[0].Type.Generics.Count, "Unexpected number of generics");
            Assert.AreEqual("CustomGeneric", objects[1].Name);
            Assert.AreEqual("SubType", objects[2].Name);
        }

        private List<ModelTransferObject> Read(Type type)
        {
            List<ITransferObject> transferObjects = new List<ITransferObject>();
            this.reader.Read(type, transferObjects);
            return transferObjects.OfType<ModelTransferObject>().ToList();
        }
    }
}