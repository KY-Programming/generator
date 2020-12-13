using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Languages
{
    public abstract class BaseLanguage : Codeable, IFormattableLanguage, IMappableLanguage, ITemplateWriter
    {
        private readonly List<ChainedCodeFragment> progressedChainedCodeFragments;
        private readonly List<ICodeFragment> lastFragments;

        protected Dictionary<Type, ITemplateWriter> TemplateWriters { get; }

        public LanguageFormatting Formatting { get; }
        public abstract string Name { get; }
        public abstract bool ImportFromSystem { get; }
        public virtual string NamespaceKeyword => "namespace";
        public virtual string ClassScope => "public";
        public virtual string PartialKeyword => "partial";
        public bool HasAbstractClasses { get; set; } = true;
        public bool HasStaticClasses { get; set; } = true;
        public IEnumerable<ICodeFragment> LastFragments => this.lastFragments;
        public object Key { get; protected set; } = new object();

        protected BaseLanguage()
        {
            this.Formatting = new LanguageFormatting();
            this.Formatting.LineClosing = ";";
            this.Formatting.IndentChar = ' ';
            this.Formatting.IdentCount = 4;
            this.Formatting.StartBlock = "{";
            this.Formatting.EndBlock = "}";
            this.Formatting.StartBlockInNewLine = true;

            this.progressedChainedCodeFragments = new List<ChainedCodeFragment>();
            this.lastFragments = new List<ICodeFragment>();

            this.TemplateWriters = new Dictionary<Type, ITemplateWriter>();
            this.TemplateWriters.Add(typeof(AccessIndexTemplate), new AccessIndexWriter());
            this.TemplateWriters.Add(typeof(AsTemplate), new AsWriter());
            this.TemplateWriters.Add(typeof(AssignTemplate), new AssignWriter());
            this.TemplateWriters.Add(typeof(BlankLineTemplate), new BlankLineWriter());
            this.TemplateWriters.Add(typeof(CaseTemplate), new CaseWriter());
            this.TemplateWriters.Add(typeof(ChainedCodeFragment), this);
            this.TemplateWriters.Add(typeof(ClassGenericTemplate), new ClassGenericWriter());
            this.TemplateWriters.Add(typeof(ClassTemplate), new ClassWriter());
            this.TemplateWriters.Add(typeof(CommentTemplate), new CommentWriter());
            this.TemplateWriters.Add(typeof(ElseTemplate), new ElseWriter());
            this.TemplateWriters.Add(typeof(ElseIfTemplate), new ElseIfWriter());
            this.TemplateWriters.Add(typeof(EnumTemplate), new EnumWriter());
            this.TemplateWriters.Add(typeof(ExecuteFieldTemplate), new ExecuteFieldWriter());
            this.TemplateWriters.Add(typeof(ExecuteGenericMethodTemplate), new ExecuteGenericMethodWriter());
            this.TemplateWriters.Add(typeof(ExecuteMethodTemplate), new ExecuteMethodWriter());
            this.TemplateWriters.Add(typeof(ExecutePropertyTemplate), new ExecutePropertyWriter());
            this.TemplateWriters.Add(typeof(FieldTemplate), new FieldWriter(this));
            this.TemplateWriters.Add(typeof(GenericTypeTemplate), new GenericTypeWriter());
            this.TemplateWriters.Add(typeof(IfTemplate), new IfWriter());
            this.TemplateWriters.Add(typeof(InlineIfTemplate), new InlineIfWriter());
            this.TemplateWriters.Add(typeof(LambdaTemplate), new LambdaWriter());
            this.TemplateWriters.Add(typeof(LocalVariableTemplate), new LocalVariableWriter());
            this.TemplateWriters.Add(typeof(MethodTemplate), new MethodWriter());
            this.TemplateWriters.Add(typeof(ExtensionMethodTemplate), new MethodWriter());
            this.TemplateWriters.Add(typeof(MultilineCodeFragment), new MultilineCodeFragmentWriter(this));
            this.TemplateWriters.Add(typeof(NamespaceTemplate), new NamespaceWriter(this));
            this.TemplateWriters.Add(typeof(NewTemplate), new NewWriter());
            this.TemplateWriters.Add(typeof(NotTemplate), new NotWriter());
            this.TemplateWriters.Add(typeof(NullValueTemplate), new NullValueWriter());
            this.TemplateWriters.Add(typeof(NullTemplate), new NullWriter());
            this.TemplateWriters.Add(typeof(NumberTemplate), new NumberWriter());
            this.TemplateWriters.Add(typeof(OperatorTemplate), new OperatorWriter());
            this.TemplateWriters.Add(typeof(PropertyTemplate), new PropertyWriter());
            this.TemplateWriters.Add(typeof(ReturnTemplate), new ReturnWriter());
            this.TemplateWriters.Add(typeof(StringTemplate), new StringWriter());
            this.TemplateWriters.Add(typeof(SwitchTemplate), new SwitchWriter());
            this.TemplateWriters.Add(typeof(ThisTemplate), new ThisWriter());
            this.TemplateWriters.Add(typeof(TypeOfTemplate), new TypeOfWriter());
            this.TemplateWriters.Add(typeof(TypeTemplate), new TypeWriter());
            this.TemplateWriters.Add(typeof(VoidTemplate), new VoidWriter());
            this.TemplateWriters.Add(typeof(AppendStringTemplate), new AppendStringWriter());
            this.TemplateWriters.Add(typeof(MathematicalOperatorTemplate), new MathWriter());
        }

        public void Write<T>(IEnumerable<T> fragments, IOutputCache output)
            where T : ICodeFragment
        {
            fragments.ForEach(fragment => this.Write(fragment, output));
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            if (fragment == null)
            {
                return;
            }
            this.lastFragments.Insert(0, fragment);
            while (this.lastFragments.Count > 10)
            {
                this.lastFragments.RemoveAt(this.lastFragments.Count - 1);
            }
            if (fragment is ChainedCodeFragment chainedCodeFragment && !this.IsProcessed(chainedCodeFragment))
            {
                this.WriteChained(chainedCodeFragment, output);
                return;
            }
            Type key = fragment.GetType();
            if (this.TemplateWriters.ContainsKey(key))
            {
                this.TemplateWriters[key].Write(fragment, output);
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

        private void WriteChained(ChainedCodeFragment fragment, IOutputCache output)
        {
            this.progressedChainedCodeFragments.Add(fragment.First());
            bool isFirst = true;
            foreach (ChainedCodeFragment codeFragment in fragment.First().Yield().Cast<ChainedCodeFragment>())
            {
                if (!isFirst)
                {
                    output.Add(codeFragment.Separator);
                }
                isFirst = false;
                this.Write(codeFragment, output);
                //output.Add(codeFragment, this);
                if (codeFragment.NewLineAfter)
                {
                    output.BreakLine();
                }
                if (codeFragment.CloseAfter)
                {
                    output.CloseLine();
                }
                if (codeFragment.BreakAfter)
                {
                    output.BreakLine().ExtraIndent();
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
                AssemblyName assemblyName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName();
                fileTemplate.Header.Description = string.Format(fileTemplate.Header.Description, $"{assemblyName.Name} {assemblyName.Version}");
            }
            FileWriter writer = new FileWriter(this);
            this.WriteHeader(fileTemplate, writer);
            StaticFileTemplate staticFile = fileTemplate as StaticFileTemplate;
            if (staticFile == null)
            {
                this.WriteUsings(fileTemplate, writer);
                this.Write(fileTemplate.Namespaces, writer);
            }
            else
            {
                writer.Add(staticFile.Content, true);
            }
            this.WriteFooter(fileTemplate, writer);
            string fileName = FileSystem.Combine(fileTemplate.RelativePath, this.FormatFileName(fileTemplate.Name, fileTemplate.IsInterface()));
            output.Write(fileName, writer.ToString(), fileTemplate.OutputId);
        }

        protected virtual void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            if (fileTemplate.Header?.Description != null)
            {
                this.Write(fileTemplate.Header, output);
                output.BreakLine();
            }
        }

        protected virtual void WriteFooter(FileTemplate fileTemplate, IOutputCache output)
        {
            if (fileTemplate.OutputIdComment != null)
            {
                output.BreakLine();
                this.Write(fileTemplate.OutputIdComment, output);
            }
        }

        protected virtual void WriteUsings(FileTemplate fileTemplate, IOutputCache output)
        {
            List<UsingTemplate> usings = this.GetUsings(fileTemplate).ToList();
            if (usings.Count <= 0)
            {
                return;
            }
            this.Write(usings, output);
            output.BreakLine();
        }

        public virtual string FormatFileName(string fileName, bool isInterface)
        {
            return fileName;
        }

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