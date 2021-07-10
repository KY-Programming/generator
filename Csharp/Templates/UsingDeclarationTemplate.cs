using KY.Generator.Templates;

namespace KY.Generator.Csharp.Templates
{
    public class UsingDeclarationTemplate : ICodeFragment
    {
        public ICodeFragment Code { get; }

        public UsingDeclarationTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}
