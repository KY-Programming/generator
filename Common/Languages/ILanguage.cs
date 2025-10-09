using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages;

public interface ILanguage
{
    string Name { get; }
    bool ImportFromSystem { get; }
    Dictionary<string, string> ReservedKeywords { get; }
    bool IsGenericTypeWithSameNameAllowed { get; }
    FormattingOptions Formatting { get; }

    void Write(ICodeFragment code, IOutputCache output);
    void Write(IEnumerable<ICodeFragment> code, IOutputCache output);
    string FormatFile(string name, GeneratorOptions options, string type = null, bool force = false);
}