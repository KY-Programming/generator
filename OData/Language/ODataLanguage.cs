using System;
using System.Collections.Generic;
using KY.Core.Meta;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.OData.Language
{
    public class ODataLanguage : ILanguage
    {
        public string Name => "OData";

        public void Write(FileTemplate file, IOutput output)
        {
            throw new NotImplementedException();
        }

        public void Write(IMetaFragmentList fragments, ICodeFragment code)
        {
            throw new NotImplementedException();
        }

        public void Write(IMetaElementList elements, ICodeFragment code)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code) where T : ICodeFragment
        {
            throw new NotImplementedException();
        }

        public void Write<T>(IMetaElementList elements, IEnumerable<T> code) where T : ICodeFragment
        {
            throw new NotImplementedException();
        }
    }
}