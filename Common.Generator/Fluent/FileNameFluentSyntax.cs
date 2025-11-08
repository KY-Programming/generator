using KY.Generator.Models;

namespace KY.Generator;

public class FileNameFluentSyntax : IFileNameFluentSyntax
{
    private readonly GeneratorOptions options;

    public FileNameFluentSyntax(GeneratorOptions options)
    {
        this.options = options;
    }

    public IFileNameFluentSyntax Replace(string pattern, string replacement, string matchingType = null)
    {
        this.options.Formatting.Add(new FileNameReplacer(null, pattern, replacement, matchingType));
        return this;
    }
}
