using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using CoreCommentWriter = KY.Generator.Writers.CommentWriter;

namespace KY.Generator.Csharp.Writers
{
    public class CommentWriter : CoreCommentWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            CommentTemplate comment = (CommentTemplate)fragment;
            if (comment.IsEmpty())
            {
                return;
            }

            if (comment.Type == CommentType.Summary)
            {
                output.Add("/// <summary>").BreakLine();
                this.SplitLines(comment.Description).ForEach(x => output.Add($"/// {x}".Trim()).BreakLine());
                output.Add("/// </summary>").BreakLine();
            }
            else
            {
                base.Write(fragment, output);
            }
        }
    }
}