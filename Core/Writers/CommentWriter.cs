using System;
using KY.Core;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class CommentWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            CommentTemplate comment = (CommentTemplate)fragment;
            if (comment.IsEmpty())
                return;

            string[] lines = comment.Description.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            lines.ForEach(x => elements.AddUnclosed().Code.Add("// ").Add(x));
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}