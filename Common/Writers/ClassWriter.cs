using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ClassWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            BaseLanguage language = output.Language.CastTo<BaseLanguage>();
            ClassTemplate template = (ClassTemplate)fragment;
            output.Add(template.Comment);
            output.Add(template.Attributes);
            output.Add(language.ClassScope).Add(" ");
            if (template.IsAbstract && language.HasAbstractClasses && !template.IsInterface)
            {
                output.Add("abstract ");
            }
            if (template.IsStatic && language.HasStaticClasses)
            {
                output.Add("static ");
            }
            else if (!string.IsNullOrEmpty(language.PartialKeyword))
            {
                output.Add(language.PartialKeyword).Add(" ");
            }
            if (template.IsInterface)
            {
                output.Add("interface ");
            }
            else
            {
                output.Add("class ");
            }
            output.Add(template.Name);
            if (template.Generics.Count > 0)
            {
                output.Add("<").Add(template.Generics.Select(x => Code.Instance.Type(x.Name)), ", ").Add(">");
            }
            template.BasedOn.OrderBy(x => x.ToType().IsInterface).ForEach(x => output.Add(x));
            output.Add(template.Generics.Select(x => x.ToConstraints()).Where(x => x.Types.Count > 0));
            output.StartBlock();
            if (template.IsInterface)
            {
                template.Fields.ForEach(x => x.Visibility = Visibility.None);
                template.Properties.ForEach(x => x.Visibility = Visibility.None);
            }
            bool isFirst = true;
            if (template.Classes.Count > 0)
            {
                output.Add(template.Classes);
                isFirst = false;
            }
            if (template.Fields.Count > 0)
            {
                output.If(!isFirst).BreakLine().EndIf();
                output.Add(template.Fields);
                isFirst = false;
            }
            if (template.Properties.Count > 0)
            {
                output.If(!isFirst).BreakLine().EndIf();
                output.Add(template.Properties);
                isFirst = false;
            }
            if (template.Code != null)
            {
                output.If(!isFirst).BreakLine().EndIf();
                output.Add(template.Code);
                isFirst = false;
            }
            if (template.Methods.Count > 0)
            {
                output.If(!isFirst).BreakLine().EndIf();
                MethodTemplate last = template.Methods.Last();
                foreach (MethodTemplate method in template.Methods)
                {
                    output.Add(method);
                    if (method != last)
                    {
                        output.BreakLine();
                    }
                }
            }
            output.EndBlock();
        }
    }
}