using CustomModule.Commands;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;

namespace CustomModule
{
    public class Module : ModuleBase
    {
        // 1. Install KY.Generator.Core nuget package
        // 2. Install KY.Generator.Fluent nuget package
        // 3. Install KY.Generator.CSharp nuget package
        // 4. Create a new Module (e.g. Module.cs, but name doesn't matters)
        //  a. Derive from KY.Core.Module.ModuleBase
        //  b. Implement a constructor
        // 5. Create parameters for command
        //  => continue on /Commands/WriteCommandParameters.cs
        // 6. Create a Writer
        //  => continue on /Writers/Writer.cs
        // 7. Create a Command
        //  => continue on /Commands/WriteCommand.cs
        // 8. Register your command (for CLI)
        //  a. Bind your command in Module.cs constructor
        // 9. Create a Write action
        // => continue on /Syntax/WriteFluentSyntaxExtension.cs

        public Module(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<WriteCommand>();
        }
    }
}
