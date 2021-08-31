using KY.Core.Dependency;
using KY.Generator.Csharp.Templates;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Languages
{
    public class CsharpLanguage : BaseLanguage
    {
        public override string Name => "Csharp";
        public override bool ImportFromSystem => true;
        public override bool IsGenericTypeWithSameNameAllowed => true;

        public CsharpLanguage(IDependencyResolver resolver)
            : base(resolver)
        {
            this.Formatting.InterfacePrefix = "I";

            this.AddWriter<AttributeTemplate, AttributeWriter>();
            this.AddWriter<BaseTypeTemplate, BaseTypeWriter>();
            this.AddWriter<BaseTemplate, BaseWriter>();
            this.AddWriter<CastTemplate, CastWriter>();
            this.AddWriter<CommentTemplate, CommentWriter>();
            this.AddWriter<ConstructorTemplate, ConstructorWriter>();
            this.AddWriter<ConstraintTemplate, ConstraintWriter>();
            this.AddWriter<CsharpTemplate, CsharpWriter>();
            this.AddWriter<DeclareTemplate, DeclareWriter>();
            this.AddWriter<GenericTypeTemplate, CsharpGenericTypeWriter>();
            this.AddWriter<ParameterTemplate, ParameterWriter>();
            this.AddWriter<ThrowTemplate, ThrowWriter>();
            this.AddWriter<UsingTemplate, UsingWriter>();
            this.AddWriter<UsingDeclarationTemplate, UsingDeclarationWriter>();
            this.AddWriter<VerbatimStringTemplate, VerbatimStringWriter>();
            this.AddWriter<ClassTemplate, CsharpClassWriter>();
            this.AddWriter<YieldReturnTemplate, YieldReturnWriter>();
            this.AddWriter<FileTemplate, CsharpFileWriter>();
        }

        public override string FormatFile(string name, IOptions options, string type = null, bool force = false)
        {
            return base.FormatFile(name, options, type, force) + ".cs";
        }
    }
}
