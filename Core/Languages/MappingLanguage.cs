using System;
using System.Collections.Generic;
using KY.Core.Meta;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public abstract class MappingLanguage : ILanguage
    {
        public abstract string Name { get; }

        public virtual void Write(FileTemplate file, IOutput output)
        {
            throw new NotImplementedException();
        }

        public virtual  void Write(IMetaFragmentList fragments, ICodeFragment code)
        {
            throw new NotImplementedException();
        }

        public virtual  void Write(IMetaElementList elements, ICodeFragment code)
        {
            throw new NotImplementedException();
        }

        public virtual  void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code) where T : ICodeFragment
        {
            throw new NotImplementedException();
        }

        public virtual  void Write<T>(IMetaElementList elements, IEnumerable<T> code) where T : ICodeFragment
        {
            throw new NotImplementedException();
        }
    }
}