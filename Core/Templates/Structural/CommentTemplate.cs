namespace KY.Generator.Templates
{
    public enum CommentType
    {
        Block,
        Line,
        Summary
    }

    public class CommentTemplate : ICodeFragment
    {
        public string Description { get; set; }
        public CommentType Type { get; set; }

        public CommentTemplate(string description = null, CommentType type = CommentType.Block)
        {
            this.Description = description;
            this.Type = type;
        }
    }
}