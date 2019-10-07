using System;
using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class CommentWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            CommentTemplate comment = (CommentTemplate)fragment;
            if (comment.IsEmpty())
            {
                return;
            }
            string[] lines = comment.Description.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            lines.ForEach(line => output.Add("// " + line).BreakLine());
        }
    }
}