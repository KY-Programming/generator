using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Output;

namespace KY.Generator.Meta
{
    internal class MetaGenerator
    {
        private readonly FileWriter fileWriter;
        private readonly Dictionary<Type, Action<MetaElement, MetaElement>> actions;

        public MetaFormatting Formatting { get; set; }

        public MetaGenerator(FileWriter fileWriter, MetaFormatting formatting = null)
        {
            this.fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
            this.Formatting = formatting ?? new MetaFormatting();
            this.actions = new Dictionary<Type, Action<MetaElement, MetaElement>>();
            this.actions[typeof(MetaStatement)] = (element, previousElement) => this.Generate((MetaStatement)element, previousElement);
            this.actions[typeof(MetaBlock)] = (element, previousElement) => this.Generate((MetaBlock)element, previousElement);
            this.actions[typeof(MetaBlankLine)] = (element, previousElement) => this.Generate((MetaBlankLine)element, previousElement);
        }

        public void Generate(IEnumerable<MetaElement> elements)
        {
            this.Generate(elements, null);
        }

        private void Generate(IEnumerable<MetaElement> elements, MetaElement previousElement)
        {
            foreach (MetaElement element in elements)
            {
                this.GenerateByType(element, previousElement);
                previousElement = element;
            }
        }

        private void GenerateByType(MetaElement element, MetaElement previousElement)
        {
            Type type = element.GetType();
            if (!this.actions.ContainsKey(type))
            {
                throw new InvalidOperationException($"No generate action for {type.Name} found");
            }
            this.actions[type](element, previousElement);
        }

        private void Generate(IEnumerable<MetaFragment> fragments)
        {
            fragments = fragments.Where(x => !string.IsNullOrEmpty(x.Code));
            bool isWrapped = false;
            MetaFragment previousFragment = null;
            foreach (MetaFragment fragment in fragments)
            {
                if (isWrapped && previousFragment.BreakAfter)
                {
                    this.fileWriter.AppendLine()
                        .Append(this.Indent(fragment.Level + 1));
                }
                this.fileWriter.Append(fragment.Code);
                isWrapped |= fragment.BreakAfter;
                previousFragment = fragment;
            }
        }

        private void Generate(MetaStatement statement, MetaElement previousElement)
        {
            this.fileWriter.Append(this.Indent(statement.Level));
            this.Generate(statement.Code);
            this.fileWriter.AppendLine(statement.IsClosed ? this.Formatting.LineCloseing : string.Empty);
        }

        private void Generate(MetaBlock statement, MetaElement previousElement)
        {
            MetaElement firstHeHeaderElement = statement.Header.FirstOrDefault();
            foreach (MetaElement element in statement.Header)
            {
                if (element == firstHeHeaderElement)
                {
                    element.UseParentLevel = true;
                }
                this.GenerateByType(element, previousElement);
                previousElement = element;
            }
            if (!statement.Skip)
            {
                if (this.Formatting.StartBlockInNewLine)
                {
                    this.fileWriter.Append(this.Indent(statement.Level));
                }
                else
                {
                    this.fileWriter.TrimEnd().Append(" ");
                }
            }
            if (statement.Elements.Count == 0 && !statement.Skip)
            {
                this.fileWriter.Append(this.Formatting.StartBlock)
                    .Append(" ")
                    .AppendLine(this.Formatting.EndBlock);
            }
            else
            {
                if (!statement.Skip)
                {
                    this.fileWriter.AppendLine(this.Formatting.StartBlock);
                }
                foreach (MetaElement element in statement.Elements)
                {
                    this.GenerateByType(element, previousElement);
                    previousElement = element;
                }
                if (!statement.Skip)
                {
                    this.fileWriter.Append(this.Indent(statement.Level))
                        .AppendLine(this.Formatting.EndBlock);
                }
            }
        }

        private void Generate(MetaBlankLine line, MetaElement previousElement)
        {
            if (previousElement is MetaBlankLine || previousElement == line.Parent || previousElement == null || previousElement.Level != line.Level)
                return;

            this.fileWriter.AppendLine();
        }

        private string Indent(int level)
        {
            return string.Empty.PadLeft(level * this.Formatting.IdentCount, this.Formatting.IndentChar);
        }
    }
}