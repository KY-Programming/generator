using System;
using KY.Core.Dependency;
using KY.Generator.Angular.Writers;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Languages
{
    internal class AngularTypeScriptLanguage : TypeScriptLanguage
    {
        public AngularTypeScriptLanguage(IDependencyResolver resolver)
            : base(resolver)
        {
            this.AddWriter<FileTemplate, AngularFileWriter>();
        }

        public override string FormatFile(string name, IOptions options, string type = null, bool force = false)
        {
            string fileName = base.FormatFile(name, options, type, force);
            if ("service".Equals(type, StringComparison.CurrentCultureIgnoreCase))
            {
                fileName = fileName.Replace("-service.ts", ".service.ts");
            }
            return fileName;
        }
    }
}
