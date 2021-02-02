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
            this.SplitLines(comment.Description).ForEach(line => output.Add($"// {line}".Trim()).BreakLine());
        }

        protected string[] SplitLines(string text)
        {
            return text.TrimStart('\r').TrimStart('\n').Replace("\r", string.Empty).Split('\n');
        }
    }
}