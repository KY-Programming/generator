using System.Threading.Tasks;
using KY.Generator.Csharp;
using KY.Generator.Output;
using KY.Generator.Reflection.Tests.Properties;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Reflection.Tests;

[TestClass]
public class InterfaceTest
{
    private Generator generator;
    private MemoryOutput output;

    [TestInitialize]
    public void Initialize()
    {
        this.output = new MemoryOutput();
        this.generator = Generator.Create()
                                  .PreloadModule<CsharpModule>()
                                  .PreloadModule<TypeScriptModule>()
                                  .PreloadModule<ReflectionModule>()
            // .SetOutput(this.output)
            ;
    }

    [TestMethod]
    public async Task TestOneInterface()
    {
        bool success = await this.RunByTypeName(nameof(OneInterfaceType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(2, this.output.Files.Count);
        Assert.AreEqual(Resources.one_interface_type_ts, this.output.Files["one-interface-type.ts"]);
        Assert.AreEqual(Resources.first_interface_ts, this.output.Files["first.interface.ts"]);
    }

    [TestMethod]
    public async Task TestTwoInterfaces()
    {
        bool success = await this.RunByTypeName(nameof(TwoInterfacesType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(3, this.output.Files.Count);
        Assert.AreEqual(Resources.two_interfaces_type_ts, this.output.Files["two-interfaces-type.ts"]);
        Assert.AreEqual(Resources.first_interface_ts, this.output.Files["first.interface.ts"]);
        Assert.AreEqual(Resources.second_interface_ts, this.output.Files["second.interface.ts"]);
    }

    [TestMethod]
    public async Task TestInheritInterfaces()
    {
        bool success = await this.RunByTypeName(nameof(InheritInterfaceType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(3, this.output.Files.Count);
        Assert.AreEqual(Resources.inherit_interface_type_ts, this.output.Files["inherit-interface-type.ts"]);
        Assert.AreEqual(Resources.inherit_first_interface_ts, this.output.Files["inherit-first.interface.ts"]);
        Assert.AreEqual(Resources.first_interface_ts, this.output.Files["first.interface.ts"]);
    }

    [TestMethod]
    public async Task TestBaseClassAndOneInterface()
    {
        bool success = await this.RunByTypeName(nameof(BaseClassAndOneInterfaceType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(4, this.output.Files.Count);
        Assert.AreEqual(Resources.base_class_and_one_interface_type_ts, this.output.Files["base-class-and-one-interface-type.ts"]);
        Assert.AreEqual(Resources.one_interface_type_ts, this.output.Files["one-interface-type.ts"]);
        Assert.AreEqual(Resources.first_interface_ts, this.output.Files["first.interface.ts"]);
        Assert.AreEqual(Resources.second_interface_ts, this.output.Files["second.interface.ts"]);
    }

    [TestMethod]
    public async Task TestBaseClassAndTwoInterfaces()
    {
        bool success = await this.RunByTypeName(nameof(BaseClassAndTwoInterfacesType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(5, this.output.Files.Count);
        Assert.AreEqual(Resources.base_class_and_two_interfaces_type_ts, this.output.Files["base-class-and-two-interfaces-type.ts"]);
        Assert.AreEqual(Resources.one_interface_type_ts, this.output.Files["one-interface-type.ts"]);
        Assert.AreEqual(Resources.first_interface_ts, this.output.Files["first.interface.ts"]);
        Assert.AreEqual(Resources.second_interface_ts, this.output.Files["second.interface.ts"]);
        Assert.AreEqual(Resources.third_interface_ts, this.output.Files["third.interface.ts"]);
    }

    [TestMethod]
    public async Task TestTypes()
    {
        bool success = await this.RunByTypeName(nameof(Types));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(2, this.output.Files.Count);
        Assert.AreEqual(Resources.types, this.output.Files["types.ts"]);
        Assert.AreEqual(Resources.subtype, this.output.Files["sub-type.ts"]);
    }

    [TestMethod]
    public async Task ExportedTypeWithSubGenericsTest()
    {
        bool success = await this.RunByTypeName(nameof(ExportedType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(3, this.output.Files.Count);
        Assert.AreEqual(Resources.exported_type_ts, this.output.Files["exported-type.ts"]);
        Assert.AreEqual(Resources.generic_type_ts, this.output.Files["generic-type.ts"]);
        Assert.AreEqual(Resources.inner_custom_type_ts, this.output.Files["inner-custom-type.ts"]);
    }

    [TestMethod]
    public async Task ExportedTypeWithPrimitiveTypeTest()
    {
        bool success = await this.RunByTypeName(nameof(ExportedPrimitiveType));
        Assert.AreEqual(true, success, "Generation not successful");
        Assert.AreEqual(2, this.output.Files.Count);
        Assert.AreEqual(Resources.exported_primitive_type_ts, this.output.Files["exported-primitive-type.ts"]);
        Assert.AreEqual(Resources.generic_type_ts, this.output.Files["generic-type.ts"]);
    }

    private Task<bool> RunByTypeName(string typeName)
    {
        return this.generator.SetParameters(
            "reflection",
            "-name=" + typeName,
            "-namespace=KY.Generator.Reflection.Tests",
            "-language=TypeScript",
            "-skipNamespace",
            "-propertiesToFields",
            "-formatNames",
            "-skipHeader"
        ).Run();
    }
}
