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
            BaseTypeTemplate previousBaseTypeTemplate = output.LastFragments.TakeWhile(x => !Equals(x, template.Parent)).OfType<BaseTypeTemplate>().FirstOrDefault(x => x != template);
            if (previousBaseTypeTemplate?.ToType().IsInterface == type.IsInterface)
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
