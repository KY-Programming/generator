﻿using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ExecuteGenericMethodWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ExecuteGenericMethodWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            ExecuteGenericMethodTemplate template = (ExecuteGenericMethodTemplate)fragment;

            fragments.Add(template.Name)
                     .Add("<")
                     .Add(template.Types, this.Language, ", ")
                     .Add(">(")
                     .Add(template.Parameters, this.Language, ", ")
                     .Add(")");
        }
    }
}