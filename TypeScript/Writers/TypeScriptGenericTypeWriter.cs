using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptGenericTypeWriter : GenericTypeWriter
    {
        public TypeScriptGenericTypeWriter(ILanguage language)
        : base(language)
        { }

        public override void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            GenericTypeTemplate template = (GenericTypeTemplate)fragment;
            if (template.Name == "List" || template.Name == "IList" || template.Name == "IEnumerable")
            {
                fragments.Add(template.Types.Single().Name)
                         .Add("[]");
            }
            else
            {
                base.Write(fragments, fragment);
            }
        }
    }
}