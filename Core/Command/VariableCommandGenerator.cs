using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Extension;
using KY.Generator.Output;

namespace KY.Generator.Command
{
    public abstract class VariableCommandGenerator : IGeneratorCommand
    {
        public abstract string[] Names { get; }
        public abstract string SubCommand { get; }
        public virtual List<string> RequiredParameters => new List<string>();

        public bool Generate(CommandConfiguration configuration, IOutput output)
        {
            Logger.Trace("Execute variable command...");
            if (configuration.Parameters.All(x => !x.Name.Equals(this.SubCommand, StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }
            foreach (string requiredParameter in this.RequiredParameters)
            {
                if (configuration.Parameters.All(available => !requiredParameter.Equals(available.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new ArgumentNullException(requiredParameter);
                }
            }
            this.OnGenerate(configuration, output);
            return true;
        }

        protected abstract void OnGenerate(CommandConfiguration configuration, IOutput output);

        protected TemplateSyntax GetTemplate(string template)
        {
            return new TemplateSyntax(template, this.GetType().Assembly.GetManifestResourceString(template));
        }
    }
}