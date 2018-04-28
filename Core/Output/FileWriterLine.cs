namespace KY.Generator.Output
{
    public class FileWriterLine
    {
        public string Content { get; set; }

        public FileWriterLine(string content = null)
        {
            this.Content = content;
        }

        public override string ToString()
        {
            return this.Content;
        }
    }
}