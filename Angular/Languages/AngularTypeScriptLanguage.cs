using KY.Core.Dependency;
using KY.Generator.Models;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Languages
{
    internal class AngularTypeScriptLanguage : TypeScriptLanguage
    {
        public AngularTypeScriptLanguage(IDependencyResolver resolver)
            : base(resolver)
        {
            this.Formatting.Add(new FileNameReplacer("angular-service", "^(.*)-service$", "$1.service", "service"));
        }
    }
}
