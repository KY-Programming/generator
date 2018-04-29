namespace KY.Generator.Templates
{
    public class TypeScriptTemplate : CodeFragment
    {
        public string Code { get; }

        public TypeScriptTemplate(string code)
        {
            this.Code = code;
        }
    }
}