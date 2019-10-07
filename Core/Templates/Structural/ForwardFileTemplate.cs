using KY.Generator.Languages;

namespace KY.Generator.Templates
{
    public class ForwardFileTemplate : FileTemplate
    {
        public FileTemplate File { get; }
        public ILanguage Language { get; }

        public ForwardFileTemplate(FileTemplate file, ILanguage language)
        {
            this.File = file;
            this.Language = language;
        }
    }
}