using KY.Generator.Templates;

namespace KY.Generator.Csharp.Templates
{
    public class YieldReturnTemplate : ICodeFragment
    {
        public ICodeFragment Code { get; }

        public YieldReturnTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}
