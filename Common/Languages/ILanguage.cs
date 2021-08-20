using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public interface ILanguage
    {
        string Name { get; }
        bool ImportFromSystem { get; }
        Dictionary<string, string> ReservedKeywords { get; }
        bool IsGenericTypeWithSameNameAllowed { get; }

        void Write(FileTemplate file, IOutput output);
        void Write(ICodeFragment code, IOutputCache output);
        void Write<T>(IEnumerable<T> code, IOutputCache output) where T : ICodeFragment;
    }
}
