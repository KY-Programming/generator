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
        public TypeScriptModelWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        protected override ClassTemplate WriteClass(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            ClassTemplate classTemplate = base.WriteClass(configuration, model, nameSpace, files);
            if (!model.IsAbstract && !model.IsInterface && configuration.Language.IsTypeScript())
            {
                ConstructorTemplate constructor = classTemplate.AddConstructor();
                constructor.WithParameter(Code.Generic("Partial", classTemplate.ToType()), "init", Code.Null())
                           .WithCode(Code.Static(Code.Type("Object")).Method("assign", Code.This(), Code.Local("init")).Close());
                if (classTemplate.BasedOn.Any(x => !x.ToType().IsInterface))
                {
                    // TODO: Add super parameters
                    constructor.WithSuper();
                }
            }
            return classTemplate;
        }
    }
}