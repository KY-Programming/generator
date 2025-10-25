using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptNamespaceWriter : NamespaceWriter
    {
        public TypeScriptNamespaceWriter()
        {
            this.NamespaceKeyword = "export namespace";
        }

    }
}
