using System;
using KY.Core;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class CommentWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            CommentTemplate comment = (CommentTemplate)fragment;
            if (comment.IsEmpty())
                return;

            string[] lines = comment.Description.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            lines.ForEach(x => elements.AddUnclosed().Code.Add("// ").Add(x));
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}