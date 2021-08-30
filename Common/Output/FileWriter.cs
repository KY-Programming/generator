using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KY.Generator.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Output
{
    internal class FileWriter : IOutputCache
    {
        private readonly IOptions options;
        private readonly List<ICodeFragment> lastFragments = new();
        private int indent;
        private readonly StringBuilder cache = new();
        private bool isLineClosed = true;

        public IEnumerable<ICodeFragment> LastFragments => this.lastFragments;

        public FileWriter(IOptions options)
        {
            this.options = options;
        }

        public IOutputCache Add(string code, bool keepIndent = false)
        {
            if (keepIndent)
            {
                this.cache.Append(code);
            }
            else if (code.Contains(Environment.NewLine))
            {
                if (this.isLineClosed)
                {
                    this.WriteIndent();
                }
                string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    this.cache.AppendLine(line.Trim());
                    this.WriteIndent();
                }
            }
            else
            {
                if (this.isLineClosed)
                {
                    this.WriteIndent();
                }
                this.cache.Append(code);
            }
            return this;
        }

        private void WriteIndent()
        {
            this.isLineClosed = false;
            this.cache.Append("".PadLeft(this.indent * this.options.Formatting.IndentCount, this.options.Formatting.IndentChar));
        }

        public IOutputCache ExtraIndent(int indents = 1)
        {
            if (this.isLineClosed)
            {
                this.WriteIndent();
            }
            this.cache.Append("".PadLeft(indents * this.options.Formatting.IndentCount, this.options.Formatting.IndentChar));
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
                this.Add(fragment);
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
                this.Add(fragment);
                first = false;
            }
            return this;
        }

        private void Add(ICodeFragment fragment)
        {
            this.lastFragments.Insert(0, fragment);
            while (this.lastFragments.Count > 10)
            {
                this.lastFragments.RemoveAt(this.lastFragments.Count - 1);
            }
            this.options.Language.Write(fragment, this);
        }

        public IOutputCache CloseLine()
        {
            this.cache.AppendLine(this.options.Formatting.LineClosing);
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
            if (this.cache.Length > 0 && !this.isLineClosed)
            {
                if (this.options.Formatting.StartBlockInNewLine)
                {
                    this.BreakLine();
                }
                else
                {
                    this.Add(" ");
                }
            }
            return this.Add(this.options.Formatting.StartBlock).Indent();
        }

        public IOutputCache EndBlock(bool breakLine = true)
        {
            return this.UnIndent().Add(this.options.Formatting.EndBlock)
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
            if (this.options.Formatting.EndFileWithNewLine)
            {
                this.BreakLine();
            }
            return this.cache.ToString().TrimEnd(' ', '\r', '\n');
        }
    }
}
