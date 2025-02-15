using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Transfer;

public class TypeScriptModelWriter : ModelWriter
{
    public TypeScriptModelWriter(Options options, ITypeMapping typeMapping, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
        : base(options, typeMapping, transferObjects, files)
    { }

    protected override ClassTemplate WriteClass(ModelTransferObject model, string relativePath)
    {
        GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
        ClassTemplate classTemplate = base.WriteClass(model, relativePath);
        if (!model.IsAbstract && !classTemplate.IsInterface && modelOptions.Language.IsTypeScript())
        {
            ConstructorTemplate constructor = classTemplate.AddConstructor();
            constructor.AddParameter(Code.Generic("Partial", classTemplate.ToType()), "init").Optional();
            constructor.WithCode(Code.Static(Code.Type("Object")).Method("assign", Code.This(), Code.Local("init")).Close());
            if (classTemplate.BasedOn.Any(x => !x.ToType().IsInterface))
            {
                // TODO: Add super parameters
                constructor.WithSuper();
            }
        }
        return classTemplate;
    }

    protected override FieldTemplate AddField(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        TypeScriptOptions fieldOptions = this.Options.Get<TypeScriptOptions>(member);
        FieldTemplate fieldTemplate = base.AddField(model, member, classTemplate);
        fieldTemplate.Strict = fieldOptions.Strict;
        if (fieldTemplate.DefaultValue == null && fieldOptions.Strict && !fieldTemplate.IsNullable)
        {
            fieldTemplate.DefaultValue = member.Type?.Default;
        }
        return fieldTemplate;
    }

    protected override PropertyTemplate AddProperty(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        TypeScriptOptions propertyOptions = this.Options.Get<TypeScriptOptions>(member);
        PropertyTemplate propertyTemplate = base.AddProperty(model, member, classTemplate);
        propertyTemplate.Strict = propertyOptions.Strict;
        if (propertyTemplate.DefaultValue == null && propertyOptions.Strict && !propertyTemplate.IsNullable)
        {
            propertyTemplate.DefaultValue = member.Type?.Default;
        }
        return propertyTemplate;
    }
}
