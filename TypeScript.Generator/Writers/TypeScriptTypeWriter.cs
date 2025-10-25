using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptTypeWriter : TypeWriter
    {
        public TypeScriptTypeWriter()
        {
            this.Writers["Default"] = new DefaultTypeWriter("{0}");
            this.Writers.Add("Array", new TypeScriptArrayTypeWriter());
        }
    }
}