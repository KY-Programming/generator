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

        public IEnumerable<ICodeFragment> LastFragments => this.output.LastFragments;
        public IOutputCache Add(string code, bool keepIndent = false) => this.output.Add(code, keepIndent);
        public IOutputCache Add(params ICodeFragment[] fragments) => this.output.Add(fragments);
        public IOutputCache Add(IEnumerable<ICodeFragment> fragments) => this.output.Add(fragments);
        public IOutputCache Add(IEnumerable<ICodeFragment> fragments, string separator) => this.output.Add(fragments, separator);
        public IOutputCache CloseLine() => this.output.CloseLine();
        public IOutputCache BreakLine() => this.output.BreakLine();
        public IOutputCache BreakLineIfNotEmpty() => this.output.BreakLineIfNotEmpty();
        public IOutputCache Indent() => this.output.Indent();
        public IOutputCache UnIndent() => this.output.UnIndent();
        public IOutputCache StartBlock() => this.output.StartBlock();
        public IOutputCache EndBlock(bool breakLine = true) => this.output.EndBlock(breakLine);
        public IOutputCache If(bool condition) => this.output.If(condition);
        public IOutputCache ExtraIndent(int indents = 1) => this.output.ExtraIndent(indents);
        public IOutputCache EndIf() => this.output;
    }
}
