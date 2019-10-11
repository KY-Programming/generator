using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KY.Core;
using KY.Core.Extension;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Output
{
    internal class FileWriter2 : IOutputCache
    {
        private readonly FileWriter2 parent;
        private int indent;
        private readonly StringBuilder cache;
        private bool isLineClosed;

        public IFormattableLanguage Language { get; }

        public IEnumerable<ICodeFragment> LastFragments => this.Language.CastSafeTo<BaseLanguage>()?.LastFragments;

        private FileWriter2()
        {
            this.cache = new StringBuilder();
            this.indent = 0;
            this.isLineClosed = true;
        }

        public FileWriter2(IFormattableLanguage language)
            : this()
        {
            this.Language = language;
        }

        private FileWriter2(FileWriter2 parent)
            : this()
        {
            this.parent = parent;
            this.Language = parent.Language;
        }

        public IOutputCache Add(string code, bool keepIndent = false)
        {
            if (this.isLineClosed)
            {
                this.WriteIndent();
                this.isLineClosed = false;
            }
            if (keepIndent)
            {
                this.cache.Append(code);
            }
            else if (code.Contains(Environment.NewLine))
            {
                string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    this.cache.AppendLine(line.Trim());
                    this.WriteIndent();
                }
            }
            else
            {
                this.cache.Append(code);
            }
            return this;
        }

        private IOutputCache WriteIndent()
        {
            this.cache.Append("".PadLeft(this.indent * this.Language.Formatting.IdentCount, this.Language.Formatting.IndentChar));
            return this;
        }

        public IOutputCache Add(params ICodeFragment[] fragments)
        {
            return this.Add(fragments.ToList());
        }

        public IOutputCache Add(IEnumerable<ICodeFragment> fragments)
        {
            foreach (ICodeFragment fragment in fragments.Where(x => x != null))
            {
                this.Language.Write(fragment, this);
            }
            return this;
        }

        public IOutputCache Add(IEnumerable<ICodeFragment> fragments, string separator)
        {
            bool first = true;
            foreach (ICodeFragment fragment in fragments.Where(x => x != null))
            {
                if (!first)
                {
                    this.Add(separator);
                }
                this.Language.Write(fragment, this);
                first = false;
            }
            return this;
        }

        public IOutputCache CloseLine()
        {
            this.cache.AppendLine(this.Language.Formatting.LineClosing);
            this.isLineClosed = true;
            return this;
        }

        public IOutputCache BreakLine()
        {
            this.cache.AppendLine();
            this.isLineClosed = true;
            return this;
        }

        public IOutputCache BreakLineIfNotEmpty()
        {
            if (!this.isLineClosed)
            {
                this.cache.AppendLine();
                this.isLineClosed = true;
            }
            return this;
        }

        public IOutputCache Indent()
        {
            this.indent++;
            this.BreakLineIfNotEmpty();
            return this;
        }

        public IOutputCache UnIndent()
        {
            this.indent--;
            this.BreakLineIfNotEmpty();
            return this;
        }

        public IOutputCache StartBlock()
        {
            if (this.cache.Length > 0)
            {
                if (this.Language.Formatting.StartBlockInNewLine)
                {
                    this.BreakLine();
                }
                else
                {
                    this.Add(" ");
                }
            }
            return this.Add(this.Language.Formatting.StartBlock).Indent();
        }

        public IOutputCache EndBlock(bool breakLine = true)
        {
            return this.UnIndent().Add(this.Language.Formatting.EndBlock)
                       .If(breakLine).BreakLine().EndIf();
        }

        public IOutputCache If(bool condition)
        {
            return new FileWriterCondition(this, condition);
        }

        public IOutputCache EndIf()
        {
            return this;
        }

        public override string ToString()
        {
            if (this.Language.Formatting.EndFileWithNewLine)
            {
                this.BreakLine();
            }
            return this.cache.ToString().TrimEnd(' ', '\r', '\n');
        }
    }
}