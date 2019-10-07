using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class BaseTypeWriter : ITemplateWriter
    {
        private const string implements = " implements ";
        private const string extends = " extends ";

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            BaseTypeTemplate template = (BaseTypeTemplate)fragment;
            if (template == null)
            {
                return;
            }
            
            TypeTemplate type = template.ToType();
            if (output.LastFragments.Skip(2).FirstOrDefault() is BaseTypeTemplate baseType && baseType.ToType().IsInterface == type.IsInterface)
            {
                output.Add(", ");
            }
            else
            {
                output.Add(type.IsInterface && !template.Parent.IsInterface ? implements : extends);
            }
            output.Add(type);
        }
    }
}