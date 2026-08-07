using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core.Dependency;
using KY.Generator.Common.Tests.Models;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Common.Tests;

[TestClass]
public class ModelWriterTests
{
    private IDependencyResolver resolver;
    private ModelWriter writer;
    private List<ITransferObject> transferObjects;
    private List<FileTemplate> files;

    [TestInitialize]
    public void Initialize()
    {
        this.transferObjects = new List<ITransferObject>();
        this.files = new List<FileTemplate>();
        this.resolver = new DependencyResolver();
        this.resolver.Bind<Options>().ToSingleton();
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<List<ITransferObject>>().To(this.transferObjects);
        this.resolver.Bind<IList<FileTemplate>>().To(this.files);
        Options.Register(() => new List<IOptionsFactory> { new GeneratorOptionsFactory() });
        this.writer = this.resolver.Create<ModelWriter>();
    }

    [TestMethod]
    public void NeverModelReferencedOnlyByServiceActionParameterIsReportedInErrorMessage()
    {
        ModelTransferObject neverModel = new()
        {
            Name = "NeverModel",
            Namespace = "KY.Test",
            Language = new TestLanguage(this.resolver)
        };
        this.resolver.Get<Options>().Get<GeneratorOptions>(neverModel).Never = true;
        this.transferObjects.Add(neverModel);

        HttpServiceTransferObject service = new() { Name = "SomeController" };
        HttpServiceActionTransferObject action = new()
        {
            Name = "Create",
            ReturnType = new TypeTransferObject { Name = "void", Namespace = "KY.Test" }
        };
        action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "model", Type = neverModel });
        service.Actions.Add(action);
        this.transferObjects.Add(service);

        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(() => this.writer.Write());
        StringAssert.Contains(exception.Message, "SomeController.Create", "The error should name the service action that references the forbidden model");
    }

    [TestMethod]
    public void NeverModelIsNotFalselyAttributedToAnUnrelatedModelSharingItsGeneratedName()
    {
        ModelTransferObject neverModel = new()
        {
            Name = "Account",
            Namespace = "KY.Test",
            Type = typeof(FirstDummyType),
            Language = new TestLanguage(this.resolver)
        };
        this.resolver.Get<Options>().Get<GeneratorOptions>(neverModel).Never = true;
        this.transferObjects.Add(neverModel);

        // Distinct source type that merely happens to render under the same generated name/namespace as neverModel.
        ModelTransferObject renamedModel = new()
        {
            Name = "Account",
            Namespace = "KY.Test",
            Type = typeof(SecondDummyType),
            Language = new TestLanguage(this.resolver)
        };
        this.transferObjects.Add(renamedModel);

        ModelTransferObject referencer = new()
        {
            Name = "Referencer",
            Namespace = "KY.Test",
            Language = new TestLanguage(this.resolver)
        };
        referencer.Properties.Add(new PropertyTransferObject { Name = "Prop", Type = renamedModel });
        this.transferObjects.Add(referencer);

        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(() => this.writer.Write());
        Assert.IsFalse(exception.Message.Contains("Referencer"), "Referencer only reaches a same-named but distinct type and must not be blamed");
    }

    private class FirstDummyType
    { }

    private class SecondDummyType
    { }

    [TestMethod]
    public void File()
    {
        ModelTransferObject model = new()
        {
            Name = "Test1",
            Language = new TestLanguage(this.resolver)
        };
        this.resolver.Get<Options>().Get<GeneratorOptions>(model).Language = new TestLanguage(this.resolver);
        this.transferObjects.Add(model);
        this.writer.Write();
        Assert.AreEqual(1, this.files.Count, "Unexpected number of files");
        Assert.AreEqual("Test1", this.files[0].Name, "Unexpected file name");
    }

    // [TestMethod]
    // public void Namespace()
    // {
    //     TestConfiguration configuration = new();
    //     List<ModelTransferObject> transferObjects = new();
    //     ModelTransferObject model = new()
    //                                 {
    //                                     Namespace = "KY.Test",
    //                                     Language = new TestLanguage()
    //                                 };
    //     transferObjects.Add(model);
    //     List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
    //     Assert.AreEqual(1, files[0].Namespaces.Count, "Unexpected number of namespace");
    //     Assert.AreEqual("KY.Test", files[0].Namespaces[0].Name, "Unexpected namespace");
    // }
    //
    // [TestMethod]
    // public void ClassName()
    // {
    //     TestConfiguration configuration = new();
    //     List<ModelTransferObject> transferObjects = new();
    //     ModelTransferObject model = new()
    //                                 {
    //                                     Name = "Test1",
    //                                     Language = new TestLanguage()
    //                                 };
    //     transferObjects.Add(model);
    //     List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
    //     Assert.AreEqual("Test1", files[0].Namespaces[0].Children[0].Name, "Unexpected class name");
    // }
    //
    // [TestMethod]
    // public void OneField()
    // {
    //     TestConfiguration configuration = new();
    //     List<ModelTransferObject> transferObjects = new();
    //     ModelTransferObject model = new()
    //                                 {
    //                                     Name = "Test1",
    //                                     Namespace = "KY.Test",
    //                                     Language = new TestLanguage()
    //                                 };
    //     model.Fields.Add(new FieldTransferObject { Name = "Field1", Type = new TypeTransferObject { Name = "string" } });
    //     transferObjects.Add(model);
    //     List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
    //     ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
    //     Assert.AreEqual(1, classTemplate.Fields.Count, "Unexpected number of fields");
    //     Assert.AreEqual("Field1", classTemplate.Fields[0].Name, "Unexpected field name");
    //     Assert.AreEqual("string", classTemplate.Fields[0].Type.Name, "Unexpected field type");
    //     Assert.AreEqual(0, classTemplate.Properties.Count, "Unexpected number of properties");
    // }
    //
    // [TestMethod]
    // public void OneProperty()
    // {
    //     TestConfiguration configuration = new();
    //     List<ModelTransferObject> transferObjects = new();
    //     ModelTransferObject model = new()
    //                                 {
    //                                     Name = "Test1",
    //                                     Namespace = "KY.Test",
    //                                     Language = new TestLanguage()
    //                                 };
    //     model.Properties.Add(new PropertyTransferObject { Name = "Prop1", Type = new TypeTransferObject { Name = "string" } });
    //     transferObjects.Add(model);
    //     List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
    //     ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
    //     Assert.AreEqual(1, classTemplate.Properties.Count, "Unexpected number of properties");
    //     Assert.AreEqual("Prop1", classTemplate.Properties[0].Name, "Unexpected property name");
    //     Assert.AreEqual("string", classTemplate.Properties[0].Type.Name, "Unexpected property type");
    //     Assert.AreEqual(0, classTemplate.Fields.Count, "Unexpected number of fields");
    // }
    //
    // [TestMethod]
    // public void OnePropertyWithDifferentNamespace()
    // {
    //     TestConfiguration configuration = new();
    //     List<ModelTransferObject> transferObjects = new();
    //     ModelTransferObject model = new()
    //                                 {
    //                                     Name = "Test1",
    //                                     Namespace = "KY.Test",
    //                                     Language = new TestLanguage()
    //                                 };
    //     model.Properties.Add(new PropertyTransferObject { Name = "Prop1", Type = new TypeTransferObject { Name = "Test2", Namespace = "KY.Test.Different" } });
    //     transferObjects.Add(model);
    //     List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
    //     ClassTemplate classTemplate = (ClassTemplate)files[0].Namespaces[0].Children[0];
    //     Assert.AreEqual(1, classTemplate.Properties.Count, "Unexpected number of properties");
    //     Assert.AreEqual("Prop1", classTemplate.Properties[0].Name, "Unexpected property name");
    //     Assert.AreEqual("Test2", classTemplate.Properties[0].Type.Name, "Unexpected property type");
    //     Assert.AreEqual(1, classTemplate.Usings.Count, "Unexpected number of usings");
    //     Assert.AreEqual("KY.Test.Different", classTemplate.Usings[0].Namespace, "Unexpected property namespace");
    //     Assert.AreEqual(0, classTemplate.Fields.Count, "Unexpected number of fields");
    // }
}
