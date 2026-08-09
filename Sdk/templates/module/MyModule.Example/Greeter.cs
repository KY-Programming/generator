using MyModule;

namespace MyModule.Example;

/// <summary>
/// This is what using the module looks like. On every build the generator writes Output/GreeterGreeter.cs
/// next to it - which is compiled into this project, so a broken generator breaks the build.
///
/// The fluent alternative to this attribute is a class deriving from GeneratorFluentMain:
///
///     this.Write(write => write.SampleModule(my => my.Hello("Hello from MyModule", "Greeter", "MyModule.Example", "Output")));
/// </summary>
[GenerateHello("Hello from MyModule", RelativePath = "Output")]
public class Greeter
{
}
