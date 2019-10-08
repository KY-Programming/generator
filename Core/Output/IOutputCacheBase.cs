using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Output
{
    public interface IOutputCacheBase
    {
        IFormattableLanguage Language { get; }
        IEnumerable<ICodeFragment> LastFragments { get; }

        IOutputCache Add(string code, bool keepIndent = false);
        IOutputCache Add(params ICodeFragment[] fragments);
        IOutputCache Add(IEnumerable<ICodeFragment> fragments);
        IOutputCache Add(IEnumerable<ICodeFragment> fragments, string separator);
        IOutputCache CloseLine();
        IOutputCache BreakLine();
        IOutputCache BreakLineIfNotEmpty();
        IOutputCache Indent();
        IOutputCache UnIndent();
        IOutputCache StartBlock();
        IOutputCache EndBlock(bool breakLine = true);
    }
}