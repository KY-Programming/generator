using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates
{
    public class TypeScriptTemplate : ICodeFragment
    {
        public string Code { get; }
        public bool BreakAfter { get; set; }
        public bool CloseAfter { get; set; }
        public bool StartBlockAfter { get; set; }
        public bool EndBlockAfter { get; set; }
        public bool IndentAfter { get; set; }
        public bool UnindentAfter { get; set; }

        public TypeScriptTemplate(string code)
        {
            this.Code = code;
        }
    }
}