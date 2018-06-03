using KY.Core.Meta;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptEnumWriter : EnumWriter
    {
        public TypeScriptEnumWriter(BaseLanguage language)
            : base(language)
        { }

        public override void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            EnumTemplate template = (EnumTemplate)fragment;
            template.BasedOn = null;
            base.Write(elements, fragment);
        }
    }
}