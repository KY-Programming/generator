using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configurations;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Transfer
{
    public class TypeScriptModelWriter : ModelWriter
    {
        public bool Strict { get; set; }

        public TypeScriptModelWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        public override List<FileTemplate> Write(IModelConfiguration configuration, IEnumerable<ITransferObject> transferObjects)
        {
            List<ITransferObject> list = transferObjects.ToList();
            TsConfig tsConfig = list.OfType<TsConfig>().FirstOrDefault();
            this.Strict = tsConfig?.CompilerOptions?.Strict ?? false;
            return base.Write(configuration, list);
        }

        protected override ClassTemplate WriteClass(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            ClassTemplate classTemplate = base.WriteClass(configuration, model, nameSpace, files);
            if (!model.IsAbstract && !classTemplate.IsInterface && configuration.Language.IsTypeScript())
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

        protected override FieldTemplate AddField(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration)
        {
            FieldTemplate fieldTemplate = base.AddField(model, name, type, classTemplate, configuration);
            if (fieldTemplate.DefaultValue == null && this.Strict)
            {
                if (fieldTemplate.Type.IsNullable)
                {
                    fieldTemplate.Type.Name += " | undefined";
                }
                else
                {
                    fieldTemplate.DefaultValue = type.Default;
                }
            }
            return fieldTemplate;
        }

        protected override PropertyTemplate AddProperty(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration, bool canRead = true, bool canWrite = true)
        {
            PropertyTemplate propertyTemplate = base.AddProperty(model, name, type, classTemplate, configuration, canRead, canWrite);
            if (propertyTemplate.DefaultValue == null && this.Strict)
            {
                if (propertyTemplate.Type.IsNullable)
                {
                    propertyTemplate.Type.Name += " | undefined";
                }
                else
                {
                    propertyTemplate.DefaultValue = type.Default;
                }
            }
            return propertyTemplate;
        }
    }
}
