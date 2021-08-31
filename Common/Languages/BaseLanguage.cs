using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;
using FileWriter = KY.Generator.Writers.FileWriter;
using OutputFileWriter = KY.Generator.Output.FileWriter;

namespace KY.Generator.Languages
{
    public abstract class BaseLanguage : Codeable, ILanguage, ITemplateWriter
    {
        private readonly IDependencyResolver resolver;
        private Dictionary<Type, Type> TemplateWriters { get; } = new();
        private Dictionary<Type, ITemplateWriter> TemplateWritersSingletons { get; } = new();

        public FormattingOptions Formatting { get; } = new();
        public abstract string Name { get; }
        public abstract bool ImportFromSystem { get; }
        public Dictionary<string, string> ReservedKeywords { get; } = new();
        public abstract bool IsGenericTypeWithSameNameAllowed { get; }
        public virtual string ClassScope => "public";
        public virtual string PartialKeyword => "partial";
        public bool HasAbstractClasses { get; set; } = true;
        public bool HasStaticClasses { get; set; } = true;

        protected BaseLanguage(IDependencyResolver resolver)
        {
            this.resolver = resolver;

            this.ReservedKeywords.Add("new", "newValue");

            this.Formatting.Add(new FileNameReplacer("interface-prefix", "^i-(.*)$", "$1"));

            this.AddWriter<AccessIndexTemplate, AccessIndexWriter>();
            this.AddWriter<AsTemplate, AsWriter>();
            this.AddWriter<AssignTemplate, AssignWriter>();
            this.AddWriter<BlankLineTemplate, BlankLineWriter>();
            this.AddWriter<CaseTemplate, CaseWriter>();
            this.AddWriter<ChainedCodeFragment, ChainedCodeFragmentWriter>(true);
            this.AddWriter<ClassGenericTemplate, ClassGenericWriter>();
            this.AddWriter<ClassTemplate, ClassWriter>();
            this.AddWriter<CommentTemplate, CommentWriter>();
            this.AddWriter<ElseTemplate, ElseWriter>();
            this.AddWriter<ElseIfTemplate, ElseIfWriter>();
            this.AddWriter<EnumTemplate, EnumWriter>();
            this.AddWriter<ExecuteFieldTemplate, ExecuteFieldWriter>();
            this.AddWriter<ExecuteGenericMethodTemplate, ExecuteGenericMethodWriter>();
            this.AddWriter<ExecuteMethodTemplate, ExecuteMethodWriter>();
            this.AddWriter<ExecutePropertyTemplate, ExecutePropertyWriter>();
            this.AddWriter<FieldTemplate, FieldWriter>();
            this.AddWriter<GenericTypeTemplate, GenericTypeWriter>();
            this.AddWriter<IfTemplate, IfWriter>();
            this.AddWriter<InlineIfTemplate, InlineIfWriter>();
            this.AddWriter<LambdaTemplate, LambdaWriter>();
            this.AddWriter<LocalVariableTemplate, LocalVariableWriter>();
            this.AddWriter<MethodTemplate, MethodWriter>();
            this.AddWriter<ExtensionMethodTemplate, MethodWriter>();
            this.AddWriter<MultilineCodeFragment, MultilineCodeFragmentWriter>();
            this.AddWriter<NamespaceTemplate, NamespaceWriter>();
            this.AddWriter<NewTemplate, NewWriter>();
            this.AddWriter<NotTemplate, NotWriter>();
            this.AddWriter<NullValueTemplate, NullValueWriter>();
            this.AddWriter<NullTemplate, NullWriter>();
            this.AddWriter<NullConditionalTemplate, NullConditionalWriter>();
            this.AddWriter<NumberTemplate, NumberWriter>();
            this.AddWriter<DateTimeTemplate, DateTimeWriter>();
            this.AddWriter<BooleanTemplate, BooleanWriter>();
            this.AddWriter<OperatorTemplate, OperatorWriter>();
            this.AddWriter<PropertyTemplate, PropertyWriter>();
            this.AddWriter<ReturnTemplate, ReturnWriter>();
            this.AddWriter<StringTemplate, StringWriter>();
            this.AddWriter<SwitchTemplate, SwitchWriter>();
            this.AddWriter<ThisTemplate, ThisWriter>();
            this.AddWriter<TypeOfTemplate, TypeOfWriter>();
            this.AddWriter<TypeTemplate, TypeWriter>();
            this.AddWriter<VoidTemplate, VoidWriter>();
            this.AddWriter<AppendStringTemplate, AppendStringWriter>();
            this.AddWriter<AppendAssignStringTemplate, AppendAssignStringWriter>();
            this.AddWriter<MathematicalOperatorTemplate, MathWriter>();
            this.AddWriter<WhileTemplate, WhileWriter>();
            this.AddWriter<NullCoalescingTemplate, NullCoalescingWriter>();
            this.AddWriter<ParenthesisTemplate, ParenthesisWriter>();
            this.AddWriter<FileTemplate, FileWriter>();
        }

        protected void AddWriter<TTemplate, TWriter>(bool singleton = false)
            where TTemplate : ICodeFragment
            where TWriter : ITemplateWriter
        {
            this.TemplateWriters[typeof(TTemplate)] = typeof(TWriter);
            if (singleton)
            {
                this.TemplateWritersSingletons[typeof(TTemplate)] = null;
            }
        }

        protected ITemplateWriter GetWriter<TTemplate>()
        {
            return this.GetWriter(typeof(TTemplate));
        }

        protected ITemplateWriter GetWriter(ICodeFragment fragment)
        {
            return this.GetWriter(fragment.GetType());
        }

        protected ITemplateWriter GetWriter(Type template)
        {
            if (template == null)
            {
                return null;
            }
            if (this.TemplateWritersSingletons.ContainsKey(template))
            {
                if (this.TemplateWritersSingletons[template] == null)
                {
                    this.TemplateWritersSingletons[template] = (ITemplateWriter)this.resolver.Create(this.TemplateWriters[template]);
                }
                return this.TemplateWritersSingletons[template];
            }
            if (this.TemplateWriters.ContainsKey(template))
            {
                return (ITemplateWriter)this.resolver.Create(this.TemplateWriters[template]);
            }
            return this.GetWriter(template.BaseType);
        }

        public void Write(IEnumerable<ICodeFragment> fragments, IOutputCache output)
        {
            fragments.ForEach(fragment => this.Write(fragment, output));
        }

        public virtual string FormatFile(string name, IOptions options, string type = null, bool force = false)
        {
            name = Formatter.Format(name, options.Formatting.FileCase, options, force);
            foreach (FileNameReplacer replacer in options.Formatting.FileNameReplacer.Where(x => x.MatchingType == null || x.MatchingType == type))
            {
                name = Regex.Replace(name, replacer.Pattern, replacer.Replacement);
            }
            return name;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            if (fragment == null)
            {
                return;
            }
            ChainedCodeFragmentWriter chainedCodeFragmentWriter = this.GetWriter<ChainedCodeFragment>().CastTo<ChainedCodeFragmentWriter>();
            if (fragment is ChainedCodeFragment chainedCodeFragment && !chainedCodeFragmentWriter.IsProcessed(chainedCodeFragment))
            {
                chainedCodeFragmentWriter.Write(chainedCodeFragment, output);
                return;
            }
            ITemplateWriter writer = this.GetWriter(fragment);
            if (writer == null)
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {fragment.GetType().Name} is not implemented in {this.Name}.");
            }
            writer.Write(fragment, output);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
