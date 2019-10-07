using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.Angular.Commands
{
    internal class GenerateAngularConfig : VariableCommandGenerator
    {
        public override string[] Names { get; } = { "generate" };
        public override string SubCommand => "angular-config";
        public override List<string> RequiredParameters => new List<string> { "url" };

        protected override void OnGenerate(CommandConfiguration configuration, IOutput output)
        {
            this.GetTemplate("generator.json")
                .SetVariable("url", configuration.Parameters.GetString("url"))
                .ToFile(false)
                .Write(output);
        }
    }
}