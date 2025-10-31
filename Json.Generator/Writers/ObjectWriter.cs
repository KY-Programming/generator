using KY.Generator.Csharp.Extensions;
using KY.Generator.Json.Transfers;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Json.Writers;

internal class ObjectWriter : ModelWriter
{
    private bool WithReader { get; set; }

    public ObjectWriter(Options options, ITypeMapping typeMapping, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
        : base(options, typeMapping, transferObjects, files)
    { }

    public void Write(bool withReader)
    {
        this.WithReader = withReader;
        base.Write();
    }

    private void WriteReader(ClassTemplate classTemplate, ModelTransferObject model)
    {
        GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
        TypeTemplate objectType = Code.Type(model.Name, model.Namespace);
        if (model.Namespace != classTemplate.Namespace.Name && model.Namespace != null)
        {
            classTemplate.AddUsing(model.Namespace);
        }
        classTemplate.WithUsing("Newtonsoft.Json")
                     //.WithUsing("Newtonsoft.Json.Linq")
                     .WithUsing("System.IO");

        classTemplate.AddMethod("Load", objectType)
                     .FormatName(modelOptions)
                     .WithParameter(Code.Type("string"), "fileName")
                     .Static()
                     .Code.AddLine(Code.Return(Code.Method("Parse", Code.Local("File").Method("ReadAllText", Code.Local("fileName")))));

        classTemplate.AddMethod("Parse", objectType)
                     .FormatName(modelOptions)
                     .WithParameter(Code.Type("string"), "json")
                     .Static()
                     .Code.AddLine(Code.Return(Code.Local("JsonConvert").GenericMethod("DeserializeObject", objectType, Code.Local("json"))));
    }

    protected override ClassTemplate WriteClass(ModelTransferObject model)
    {
        ClassTemplate classTemplate = base.WriteClass(model);
        if (model is JsonModelTransferObject && this.WithReader)
        {
            this.WriteReader(classTemplate, model);
        }
        return classTemplate;
    }

    protected override FieldTemplate AddField(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        FieldTemplate fieldTemplate = base.AddField(model, member, classTemplate);
        if (!fieldTemplate.Name.Equals(member.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            fieldTemplate.WithAttribute("JsonProperty", Code.String(member.Name));
            classTemplate.AddUsing("Newtonsoft.Json");
        }
        return fieldTemplate;
    }

    protected override PropertyTemplate AddProperty(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        PropertyTemplate propertyTemplate = base.AddProperty(model, member, classTemplate);
        if (!propertyTemplate.Name.Equals(member.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            propertyTemplate.WithAttribute("JsonProperty", Code.String(member.Name));
            classTemplate.AddUsing("Newtonsoft.Json");
        }
        return propertyTemplate;
    }
}
