using CustomModule.Syntax;
using KY.Generator;

namespace CustomModule.Console
{
    // 1. Install KY.Generator.Fluent nuget package
    // 2. Install/reference your custom module (like here CustomModule project)
    // 3. Derive from KY.Generator.GeneratorFluentMain
    // 4. Implement Execute method
    //  a. Use Write() method and call your new action (like here HelloWorld method)
    public class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Write().Message("Hello World!", "Program", "Hello.World", "Output");
        }
    }
}