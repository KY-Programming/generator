namespace KY.Generator.Languages
{
    public class LanguageFormatting
    {
        public string LineClosing { get; set; }
        public char IndentChar { get; set; }
        public int IdentCount { get; set; }
        public string StartBlock { get; set; }
        public string EndBlock { get; set; }
        public bool StartBlockInNewLine { get; set; }
        public bool EndFileWithNewLine { get; set; }

        public string Indent(int level)
        {
            return string.Empty.PadLeft(level * this.IdentCount, this.IndentChar);
        }
    }
}