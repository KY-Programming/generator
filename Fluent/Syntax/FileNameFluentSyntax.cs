using KY.Generator.Models;

namespace KY.Generator.Syntax
{
    public class FileNameFluentSyntax : IFileNameFluentSyntax
    {
        private readonly IOptions options;

        public FileNameFluentSyntax(IOptions options)
        {
            this.options = options;
        }

        public IFileNameFluentSyntax Replace(string pattern, string replacement, string matchingType = null)
        {
            this.options.Formatting.Add(new FileNameReplacer(null, pattern, replacement, matchingType));
            return this;
        }
    }
}