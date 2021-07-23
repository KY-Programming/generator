using System.Reflection;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CsharpClassWriter : ClassWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            AssemblyName assemblyName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName();
            ClassTemplate template = (ClassTemplate)fragment;
            AttributeTemplate attributeTemplate = template.AddAttribute("GeneratedCode", Code.Instance.String(assemblyName.Name), Code.Instance.String(assemblyName.Version.ToString()));
            base.Write(fragment, output);
            template.Attributes.Remove(attributeTemplate);
        }
    }
}
