using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core.Extension;
using KY.Generator.Templates;

namespace KY.Generator.Command
{
    public abstract class VariableCommandGenerator : ICommandGenerator
    {
        public abstract string Command { get; }
        public abstract string SubCommand { get; }
        public virtual List<string> RequiredParameters => new List<string>();

        public void Generate(CommandConfiguration configuration, IList<FileTemplate> files)
        {
            if (configuration.Parameters.All(x => !x.Name.Equals(this.SubCommand, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }
            foreach (string requiredParameter in this.RequiredParameters)
            {
                if (configuration.Parameters.All(available => !requiredParameter.Equals(available.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new ArgumentNullException(requiredParameter);
                }
            }
            this.OnGenerate(configuration, files);
        }

        protected abstract void OnGenerate(CommandConfiguration configuration, IList<FileTemplate> files);

        protected TemplateSyntax GetTemplate(string template)
        {
            return new TemplateSyntax(template, this.GetType().Assembly.GetManifestResourceString(template));
        }
    }
}