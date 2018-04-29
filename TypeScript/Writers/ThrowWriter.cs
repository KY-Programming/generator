using System;
using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ThrowWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ThrowWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ThrowTemplate template = (ThrowTemplate)fragment;
            if (template.Type.Name == nameof(ArgumentOutOfRangeException))
            {
                fragments.Add("throw new Error(")
                         .Add(template.Parameters[2], this.Language)
                         .Add(" + \" Actual value: \" + ")
                         .Add(template.Parameters[1], this.Language)
                         .Add(")");
            }
            else
            {
                fragments.Add("throw new Error(")
                         .Add(template.Parameters, this.Language)
                         .Add(")");
            }
        }
    }
}