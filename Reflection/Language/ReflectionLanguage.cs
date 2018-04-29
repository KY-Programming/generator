using System;
using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Reflection.Language
{
    public class ReflectionLanguage : ILanguage
    {
        public string Name => "Reflection";

        public void Write(FileTemplate file, IOutput output)
        {
            throw new NotImplementedException();
        }

        public void Write(IMetaFragmentList fragments, CodeFragment code)
        {
            throw new NotImplementedException();
        }

        public void Write(IMetaElementList elements, CodeFragment code)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code) where T : CodeFragment
        {
            throw new NotImplementedException();
        }

        public void Write<T>(IMetaElementList elements, IEnumerable<T> code) where T : CodeFragment
        {
            throw new NotImplementedException();
        }
    }
}