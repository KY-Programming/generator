using System.Collections.Generic;
using KY.Core.Meta;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public interface ILanguage
    {
        string Name { get; }

        void Write(FileTemplate file, IOutput output);
        void Write(IMetaFragmentList fragments, ICodeFragment code);
        void Write(IMetaElementList elements, ICodeFragment code);

        void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code)
            where T : ICodeFragment;

        void Write<T>(IMetaElementList elements, IEnumerable<T> code)
            where T : ICodeFragment;
    }
}