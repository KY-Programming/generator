using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    internal class ConstructorWriter : MethodWriter
    {
        protected override void BeforeBlock(ICodeFragment fragment, IOutputCache output)
        {
            ConstructorTemplate constructorTemplate = (ConstructorTemplate)fragment;
            if (constructorTemplate.ConstructorCall != null)
            {
                output.BreakLine()
                      .Indent()
                      .Add(": ")
                      .Add(constructorTemplate.ConstructorCall)
                      .UnIndent();
            }
        }
    }
}