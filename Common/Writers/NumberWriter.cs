using System.Globalization;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NumberWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NumberTemplate template = (NumberTemplate)fragment;
            if (template.FloatValue.HasValue)
            {
                output.Add($"{template.FloatValue?.ToString(CultureInfo.InvariantCulture)}f");
            }
            else
            {
                output.Add(template.LongValue?.ToString(CultureInfo.InvariantCulture) ?? template.DoubleValue?.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}