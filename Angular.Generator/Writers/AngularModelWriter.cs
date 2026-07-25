using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers;

public class AngularModelWriter : TypeScriptModelWriter
{
    public AngularModelWriter(Options options, ITypeMapping typeMapping, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
        : base(options, typeMapping, transferObjects, files)
    { }

    protected override void WriteModel(ModelTransferObject model)
    {
        GeneratorOptions options = this.Options.Get<GeneratorOptions>(model);
        options.ModelOutput ??= "/ClientApp/src/app/models";
        base.WriteModel(model);
    }

    protected override FieldTemplate? AddField(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        FieldTemplate? fieldTemplate = base.AddField(model, member, classTemplate);
        if (fieldTemplate == null || !this.WithSignals(model) || model.Constants.Contains(member))
        {
            return fieldTemplate;
        }
        return this.ToSignal(fieldTemplate, classTemplate);
    }

    /// <summary>
    /// Replaces the field with a signal backed field e.g. <code>name: string</code> becomes <code>name: WritableSignal&lt;string&gt;</code>.
    /// An optional field e.g. <code>api?: string</code> becomes <code>api: WritableSignal&lt;string | undefined&gt;</code>, the field itself
    /// is always present, only its value stays optional
    /// </summary>
    private FieldTemplate ToSignal(FieldTemplate fieldTemplate, ClassTemplate classTemplate)
    {
        bool isValueOptional = fieldTemplate.IsOptional
                               || fieldTemplate.IsNullable
                               || fieldTemplate.Strict && fieldTemplate.DefaultValue == null;
        TypeTemplate valueType = isValueOptional
                                     ? Code.UnionType(fieldTemplate.Type, Code.Type("undefined"))
                                     : fieldTemplate.Type;
        // The signal itself is never optional and never strict, the optionality moved into its value type
        FieldTemplate signalField = new(classTemplate, fieldTemplate.Name, Code.Generic("WritableSignal", valueType))
                                    {
                                        Visibility = fieldTemplate.Visibility,
                                        Comment = fieldTemplate.Comment,
                                        IsReadonly = fieldTemplate.IsReadonly
                                    };
        classTemplate.WithUsing("WritableSignal", "@angular/core");
        if (fieldTemplate.DefaultValue != null && !classTemplate.IsInterface)
        {
            signalField.DefaultValue = Code.Method("signal", fieldTemplate.DefaultValue);
            classTemplate.WithUsing("signal", "@angular/core");
        }
        classTemplate.Fields[classTemplate.Fields.IndexOf(fieldTemplate)] = signalField;
        return signalField;
    }

    private bool WithSignals(ModelTransferObject model)
    {
        return this.Options.Get<AngularOptions>(model).WithSignals;
    }
}
