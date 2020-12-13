using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.Fluent
{
    internal class FluentCommand : GeneratorCommand<FluentCommandParameters>
    {
        public override string[] Names => new[] { "fluent" };

        public override IGeneratorCommandResult Run(IOutput output)
        {
            return this.Success();
        }
    }
}