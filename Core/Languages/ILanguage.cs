using System.Collections.Generic;
using KY.Generator.Meta;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public interface ILanguage
    {
        string Name { get; }

        void Write(FileTemplate file, IOutput output);
        void Write(IMetaFragmentList fragments, CodeFragment code);
        void Write(IMetaElementList elements, CodeFragment code);

        void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code)
            where T : CodeFragment;

        void Write<T>(IMetaElementList elements, IEnumerable<T> code)
            where T : CodeFragment;
    }
}