using System;
using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Languages
{
    public class EmptyLanguage : IMappableLanguage
    {
        public virtual string Name => "Empty";
        public bool ImportFromSystem => true;
        public object Key { get; } = new object();

        public virtual void Write(FileTemplate file, IOutput output)
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {file.GetType().Name} is not implemented in {this.Name}.");
        }

        public virtual void Write(ICodeFragment code, IOutputCache output)
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {code.GetType().Name} is not implemented in {this.Name}.");
        }

        public virtual void Write<T>(IEnumerable<T> code, IOutputCache output) where T : ICodeFragment
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {code.GetType().Name} is not implemented in {this.Name}.");
        }
    }
}