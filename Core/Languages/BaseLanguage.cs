using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Languages
{
    public abstract class BaseLanguage : ILanguage, ITemplateWriter
    {
        private readonly List<ChainedCodeFragment> progressedChainedCodeFragments;

        protected MetaFormatting Formatting { get; }
        protected Dictionary<Type, ITemplateWriter> TemplateWriters { get; }

        public abstract string Name { get; }
        public abstract string MappingKeyword { get; }
        public virtual string NamespaceKeyword => "namespace";
        public virtual string ClassScope => "public";
        public virtual string PartialKeyword => "partial";
        public CodeFragment LastFragment { get; private set; }

        protected BaseLanguage()
        {
            this.Formatting = new MetaFormatting();
            this.Formatting.LineCloseing = ";";
            this.Formatting.IndentChar = ' ';
            this.Formatting.IdentCount = 4;
            this.Formatting.StartBlock = "{";
            this.Formatting.EndBlock = "}";
            this.Formatting.StartBlockInNewLine = true;

            this.progressedChainedCodeFragments = new List<ChainedCodeFragment>();
            
            this.TemplateWriters = new Dictionary<Type, ITemplateWriter>();
            this.TemplateWriters.Add(typeof(AssignTemplate), new AssignWriter(this));
            this.TemplateWriters.Add(typeof(CaseTemplate), new CaseWriter(this));
            this.TemplateWriters.Add(typeof(ChainedCodeFragment), this);
            this.TemplateWriters.Add(typeof(ClassGenericTemplate), new ClassGenericWriter());
            this.TemplateWriters.Add(typeof(ClassTemplate), new ClassWriter(this));
            this.TemplateWriters.Add(typeof(CommentTemplate), new CommentWriter());
            this.TemplateWriters.Add(typeof(ConstructorTemplate), new MethodWriter(this));
            this.TemplateWriters.Add(typeof(EnumTemplate), new EnumWriter(this));
            this.TemplateWriters.Add(typeof(ExecuteFieldTemplate), new ExecuteFieldWriter());
            this.TemplateWriters.Add(typeof(ExecuteGenericMethodTemplate), new ExecuteGenericMethodWriter(this));
            this.TemplateWriters.Add(typeof(ExecuteMethodTemplate), new ExecuteMethodWriter(this));
            this.TemplateWriters.Add(typeof(ExecutePropertyTemplate), new ExecutePropertyWriter());
            this.TemplateWriters.Add(typeof(FieldTemplate), new FieldWriter(this));
            this.TemplateWriters.Add(typeof(GenericTypeTemplate), new GenericTypeWriter(this));
            this.TemplateWriters.Add(typeof(LocalVariableTemplate), new LocalVariableWriter());
            this.TemplateWriters.Add(typeof(MethodTemplate), new MethodWriter(this));
            this.TemplateWriters.Add(typeof(MultilineCodeFragment), new MultilineCodeFragmentWriter(this));
            this.TemplateWriters.Add(typeof(NamespaceTemplate), new NamespaceWriter(this));
            this.TemplateWriters.Add(typeof(NewTemplate), new NewWriter(this));
            this.TemplateWriters.Add(typeof(NullValueTemplate), new NullValueWriter());
            this.TemplateWriters.Add(typeof(NullTemplate), new NullWriter());
            this.TemplateWriters.Add(typeof(NumberTemplate), new NumberWriter());
            this.TemplateWriters.Add(typeof(OperatorTemplate), new OperatorWriter());
            this.TemplateWriters.Add(typeof(PropertyTemplate), new PropertyWriter(this));
            this.TemplateWriters.Add(typeof(ReturnTemplate), new ReturnWriter(this));
            this.TemplateWriters.Add(typeof(StringTemplate), new StringWriter());
            this.TemplateWriters.Add(typeof(SwitchTemplate), new SwitchWriter(this));
            this.TemplateWriters.Add(typeof(ThisTemplate), new ThisWriter());
            this.TemplateWriters.Add(typeof(TypeOfTemplate), new TypeOfWriter(this));
            this.TemplateWriters.Add(typeof(TypeTemplate), new TypeWriter());
            this.TemplateWriters.Add(typeof(VoidTemplate), new VoidWriter());
        }

        public void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code)
            where T : CodeFragment
        {
            code.ForEach(fragment => this.Write(fragments, fragment));
        }

        public void Write(IMetaFragmentList fragments, CodeFragment code)
        {
            if (code == null)
            {
                return;
            }
            this.LastFragment = code;
            ChainedCodeFragment chainedCodeFragment = code as ChainedCodeFragment;
            if (chainedCodeFragment != null && !this.IsProcessed(chainedCodeFragment))
            {
                this.WriteChained(fragments, chainedCodeFragment);
                return;
            }
            Type key = code.GetType();
            if (this.TemplateWriters.ContainsKey(key))
            {
                this.TemplateWriters[key].Write(fragments, code);
            }
            else
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {key.Name} is not implemented in {this.Name}.");
            }
        }

        public void Write<T>(IMetaElementList elements, IEnumerable<T> code)
            where T : CodeFragment
        {
            code.ForEach(fragment => this.Write(elements, fragment));
        }

        public void Write(IMetaElementList elements, CodeFragment code)
        {
            if (code == null)
            {
                return;
            }
            this.LastFragment = code;
            ChainedCodeFragment chainedCodeFragment = code as ChainedCodeFragment;
            if (chainedCodeFragment != null && !this.IsProcessed(chainedCodeFragment))
            {
                this.WriteChained(elements, chainedCodeFragment);
                return;
            }
            Type key = code.GetType();
            if (this.TemplateWriters.ContainsKey(key))
            {
                this.TemplateWriters[key].Write(elements, code);
            }
            else
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {key.Name} is not implemented in {this.Name}.");
            }
        }

        private bool IsProcessed(ChainedCodeFragment fragment)
        {
            return this.progressedChainedCodeFragments.Contains(fragment.First());
        }

        private void WriteChained(IMetaElementList elements, ChainedCodeFragment fragment)
        {
            this.WriteChained(elements.AddClosed().Code, fragment);
        }

        private void WriteChained(IMetaFragmentList elements, ChainedCodeFragment fragment)
        {
            this.progressedChainedCodeFragments.Add(fragment.First());
            bool isFirst = true;
            foreach (ChainedCodeFragment codeFragment in fragment.First().Yield().Cast<ChainedCodeFragment>())
            {
                if (!isFirst)
                {
                    elements.Add(codeFragment.Separator);
                }
                isFirst = false;
                elements.Add(codeFragment, this);
                if (codeFragment.NewLineAfter)
                {
                    elements.AddNewLine();
                }
            }
        }

        public virtual void Write(FileTemplate fileTemplate, IOutput output)
        {
            if (string.IsNullOrEmpty(fileTemplate.Name))
            {
                Logger.Trace("Empty file skipped");
                return;
            }

            if (fileTemplate.Header.Description != null)
            {
                AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
                fileTemplate.Header.Description = string.Format(fileTemplate.Header.Description, $"{assemblyName.Name} {assemblyName.Version}");
            }
            IMetaElementList elements = new MetaElementList();
            StaticFileTemplate staticFile = fileTemplate as StaticFileTemplate;
            if (staticFile == null)
            {
                string fileName = FileSystem.Combine(fileTemplate.RelativePath, this.GetFileName(fileTemplate));
                this.Write(elements, fileTemplate.Header);
                elements.AddBlankLine();
                this.Write(elements, this.GetUsings(fileTemplate));
                elements.AddBlankLine();
                this.Write(elements, fileTemplate.Namespaces);
                FileWriter fileWriter = new FileWriter(output, fileName);
                MetaGenerator metaGenerator = new MetaGenerator(fileWriter, this.Formatting);
                metaGenerator.Generate(elements);
                fileWriter.WriteFile();
            }
            else
            {
                string fileName = FileSystem.Combine(fileTemplate.RelativePath, fileTemplate.Name);
                FileWriter fileWriter = new FileWriter(output, fileName);
                fileWriter.Append(staticFile.Content);
                fileWriter.WriteFile();
            }
        }

        public abstract string GetFileName(FileTemplate fileTemplate);
        public abstract string GetClassName(ClassTemplate classTemplate);
        public abstract string GetClassName(EnumTemplate classTemplate);
        public abstract string GetPropertyName(PropertyTemplate propertyTemplate);
        public abstract string GetFieldName(FieldTemplate fieldTemplate);
        public abstract string GetMethodName(MethodTemplate methodTemplate);
        protected abstract IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate);

        public virtual string ConvertValue(object value)
        {
            if (value == null)
            {
                return "null";
            }
            if (value is string)
            {
                return $"\"{value}\"";
            }
            return value.ToString();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}