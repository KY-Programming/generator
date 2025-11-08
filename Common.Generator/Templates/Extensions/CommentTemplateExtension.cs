namespace KY.Generator.Templates.Extensions
{
    public static class CommentTemplateExtension
    {
        public static bool IsEmpty(this CommentTemplate comment)
        {
            return comment == null || string.IsNullOrEmpty(comment.Description);
        }
    }
}