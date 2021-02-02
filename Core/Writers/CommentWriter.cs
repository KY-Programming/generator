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
            foreach (string line in this.SplitLines(comment.Description))
            {
                if (line.TrimStart().StartsWith("/*"))
                {
                    if (line.EndsWith("*/"))
                    {
                        output.Add(line);
                    }
                    else
                    {
                        output.Add(line.Replace("*/", "*/ //"));
                    }
                }
                else
                {
                    output.Add($"// {line}".Trim());
                }
                output.BreakLine();
            }
        }

        protected string[] SplitLines(string text)
        {
            return text.TrimStart('\r').TrimStart('\n').Replace("\r", string.Empty).Split('\n');
        }
    }
}