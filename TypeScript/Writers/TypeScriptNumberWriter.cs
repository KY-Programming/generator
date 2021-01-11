using System.Globalization;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptNumberWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NumberTemplate template = (NumberTemplate)fragment;
            output.Add(template.LongValue?.ToString(CultureInfo.InvariantCulture)
                       ?? template.DoubleValue?.ToString(CultureInfo.InvariantCulture)
                       ?? template.FloatValue?.ToString(CultureInfo.InvariantCulture));
        }
    }
}