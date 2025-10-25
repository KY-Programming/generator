using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates
{
    public class UnknownExportTemplate : UsingTemplate
    {
        public TypeScriptTemplate Code { get; }

        public UnknownExportTemplate(TypeScriptTemplate code)
        {
            this.Code = code;
        }
    }
}
