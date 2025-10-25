using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class NamespaceTemplateExtension
    {
        public static NamespaceTemplate WithDeclareType(this NamespaceTemplate namespaceTemplate, string name, ICodeFragment type)
        {
            namespaceTemplate.AddDeclareType(name, type);
            return namespaceTemplate;
        }

        public static DeclareTypeTemplate AddDeclareType(this NamespaceTemplate namespaceTemplate, string name, ICodeFragment type)
        {
            DeclareTypeTemplate declareTypeTemplate = new DeclareTypeTemplate(name, type);
            namespaceTemplate.Children.Insert(0, declareTypeTemplate);
            return declareTypeTemplate;
        }
    }
}