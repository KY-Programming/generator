using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core.Extension;
using KY.Generator.Command;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Angular
{
    internal class GenerateAngularConfig : VariableCommandGenerator
    {
        public override string Command => "generate";
        public override string SubCommand => "angular-config";
        public override List<string> RequiredParameters => new List<string> { "url" };

        protected override void OnGenerate(CommandConfiguration configuration, IList<FileTemplate> files)
        {
            this.GetTemplate("generator.json")
                .SetVariable("url", configuration.Parameters.GetValue("url"))
                .ToFile(false)
                .AddTo(files);
        }
    }
}