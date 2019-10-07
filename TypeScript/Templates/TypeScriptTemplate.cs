using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates
{
    public class TypeScriptTemplate : ICodeFragment
    {
        public string Code { get; }

        public TypeScriptTemplate(string code)
        {
            this.Code = code;
        }
    }
}