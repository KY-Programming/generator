using KY.Generator.Templates;

namespace KY.Generator.Csharp.Templates
{
    public class CsharpTemplate : ICodeFragment
    {
        public string Code { get; set; }

        public CsharpTemplate(string code)
        {
            this.Code = code;
        }
    }
}