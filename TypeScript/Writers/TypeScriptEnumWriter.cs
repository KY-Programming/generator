using KY.Core.Meta;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class TypeScriptEnumWriter : EnumWriter
    {
        public TypeScriptEnumWriter(BaseLanguage language)
            : base(language)
        { }

        public override void Write(IMetaElementList elements, CodeFragment fragment)
        {
            EnumTemplate template = (EnumTemplate)fragment;
            template.BasedOn = null;
            base.Write(elements, fragment);
        }
    }
}