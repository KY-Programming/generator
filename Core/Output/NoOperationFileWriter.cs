using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Output
{
    class NoOperationFileWriter : IOutputCache
    {
        private readonly IOutputCache output;

        public NoOperationFileWriter(IOutputCache output)
        {
            this.output = output;
        }

        public IFormattableLanguage Language => this.output.Language;

        public IEnumerable<ICodeFragment> LastFragments => this.output.LastFragments;

        public IOutputCache Add(string code, bool keepIndent = false)
        {
            return this.output.Add(code, keepIndent);
        }

        public IOutputCache Add(params ICodeFragment[] fragments)
        {
            return this.output.Add(fragments);
        }

        public IOutputCache Add(IEnumerable<ICodeFragment> fragments)
        {
            return this.output.Add(fragments);
        }

        public IOutputCache Add(IEnumerable<ICodeFragment> fragments, string separator)
        {
            return this.output.Add(fragments, separator);
        }

        public IOutputCache CloseLine()
        {
            return this.output.CloseLine();
        }

        public IOutputCache BreakLine()
        {
            return this.output.BreakLine();
        }

        public IOutputCache BreakLineIfNotEmpty()
        {
            return this.output.BreakLineIfNotEmpty();
        }

        public IOutputCache Indent()
        {
            return this.output.Indent();
        }

        public IOutputCache UnIndent()
        {
            return this.output.UnIndent();
        }

        public IOutputCache StartBlock()
        {
            return this.output.StartBlock();
        }

        public IOutputCache EndBlock(bool breakLine = true)
        {
            return this.output.EndBlock(breakLine);
        }

        public IOutputCache If(bool condition)
        {
            return this.output.If(condition);
        }

        public IOutputCache EndIf()
        {
            return this.output;
        }

        public IOutputCache ExtraIndent(int indents = 1)
        {
            return this.output.ExtraIndent(indents);
        }
    }
}