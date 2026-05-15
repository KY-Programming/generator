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

    /// <summary>
    /// True if this language emits class members as properties (e.g. C#), false if it emits them as fields (e.g. TypeScript).
    /// The transfer writer uses this to convert between the two when the source and target disagree.
    /// </summary>
    bool PrefersProperties { get; }

    void Write(ICodeFragment code, IOutputCache output);
    void Write(IEnumerable<ICodeFragment> code, IOutputCache output);
    string FormatFile(string name, GeneratorOptions options, string type = null, bool force = false);
}